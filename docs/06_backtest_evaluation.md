# 06. Backtest & Evaluation: 전략 검증 및 성과 분석

## 목적
과거 데이터를 활용하여 테마 로테이션 전략의 수익성과 리스크를 정량적으로 검증하고 과최적화 가능성 배제.

## 입력/출력 명세
### 입력
- **Signals**: 일별 테마별 진입/보유/청산 신호.
- **Market Price**: 수정 주가 기반 일봉 데이터.
- **Costs**: 수수료, 슬리피지, 세금 설정값.

### 출력
- **Equity Curve**: 전략 누적 수익률 곡선.
- **Performance Metrics**: CAGR, Sharpe Ratio, MDD, Turnover, Hit Rate.
- **Signal Analysis**: 신호별 평균 수익률, 보유 기간 등 기초 통계.

## 모듈 설계
### Vectorized Backtest Engine
- **Position Matrix**: Ticker별/일별 보유 비중 행렬 산출.
- **Return Calculation**: `Position * Daily_Return` 연산을 통한 전체 포트폴리오 수익률 도출.
- **Transaction Cost Model**: 포지션 변경량(Delta Weight)에 따른 비용 일괄 차감 처리.

### Walk-forward Verification
- **Rolling Window**: 학습(최적화) 구간과 검증 구간을 분리하여 전진 분석 수행.
- **Parameter Sensitivity**: 주요 임계값 변화에 따른 성과 안정성 테스트.

## 실무 포인트
- **Slippage Proxy**: 거래 대금 대비 포지션 규모를 고려한 슬리피지 추정치 적용.
- **Survival Bias Check**: 백테스트 유니버스에 상장 폐지 종목이 정확히 포함되었는지 확인.
- **Liquidity Restriction**: 거래 대금이 부족하여 실제 체결이 불가능한 시나리오 감지 및 페널티 부여.

## 예시
### 핵심 성과 지표 계산 (Formula)
- **Sharpe** = (평균 초과 수익률) / (수익률 표준편차)
- **MDD** = (현재 가치 - 이전 최고 가치) / (이전 최고 가치) 의 최솟값.

### 백테스트 코드 구조 (Vectorized)
```python
def run_vectorized_backtest(returns, weights, cost=0.003):
    """
    벡터 연산 기반 고속 백테스트 처리
    """
    # 일별 수익률 산출
    port_ret = (weights.shift(1) * returns).sum(axis=1)
    # 거래 비용 산출 (Turnover 기반)
    diff_weight = weights.diff().abs().sum(axis=1)
    trans_cost = diff_weight * cost
    # 최종 순수익률
    net_ret = port_ret - trans_cost
    return net_ret.cumsum()
```
