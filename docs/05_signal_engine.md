# 05. Signal Engine: 테마 로테이션 및 매매 신호 알고리즘

## 목적
정량적 지표를 결합하여 주도 테마 선정, 보유 테마 청산 및 차기 테마 진입에 대한 의사 결정 자동화.

## 입력/출력 명세
### 입력
- **Theme Features**: RS, Breadth, Concentration, Volatility.
- **Regime State**: 현재 시장의 리스크 국면 점수.

### 출력
- **Leader Score**: 테마별 주도력 점수.
- **Exit Level**: 주도 테마의 위험 징후 단계 (0, 1, 2).
- **Next Candidate Rank**: 차기 유망 테마 순위 및 근거.

## 모듈 설계
### Leader Detection
- **Score Calculation**: `a*RS_60 + b*Mom_20 + c*Breadth - d*ConcentrationPenalty`
- **Ranking**: 점수 상위 N개 테마를 현재 "Leader"로 정의.

### Exit Signal (하락 전조)
- **Divergence**: 가격은 신고가이나 Breadth(상승 종목 비율)가 하락하는 경우(Breadth Divergence).
- **Extreme Concentration**: 소수 대형 종목으로 매수세가 극단적으로 쏠리는 오버슈팅 포착.
- **Volatility Spike**: 추세 상단에서의 변동성 급증을 분배 구간으로 판단.

### Next Theme Ranking
- **Reversal Momentum**: 하락 추세가 진정되고 RS가 턴어라운드하는 테마 검색.
- **Statistical Correlation**: 현재 리더 테마와 역사적으로 순환 주기가 연결된 테마 선별.

## 실무 포인트
- **Cost vs Performance**: 턴오버(Turnover) 과다 발생 시 거래 비용으로 인한 수익성 악화 주의.
- **Threshold Tuning**: 선정/청산 신호 임계값에 대한 과거 데이터 집약적 최적화 필요.
- **Explainability**: 각 신호 발생 시 주요 기여 피처를 기록하여 리포트에 출력.

## 예시
### Leader Score 로직 (Python 예시)
```python
def calculate_leader_score(features):
    """
    테마별 리더 스코어 산출
    """
    score = (
        features['rs_60'] * 0.4 +
        features['breadth'] * 0.3 +
        features['mom_20'] * 0.3 -
        features['concentration_idx'] * 0.2 # 쏠림에 대한 페널티
    )
    return score
```

### Exit Levels
- **Level 1 (Caution)**: Breadth 20일 이동평균 하회.
- **Level 2 (Warning)**: RS 60일 최고점 대비 10% 하락 및 Concentration 임계치 돌파.
- **Level 3 (Exit)**: 주요 지지선 이탈 및 Regime Risk-Off 전환.
