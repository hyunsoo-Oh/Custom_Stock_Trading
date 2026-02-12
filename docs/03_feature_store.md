# 03. Feature Store: 종목 및 테마 팩터 계산 로직

## 목적
테마 로테이션과 하락 전조 감지를 위한 핵심 정량 지표(Feature) 생성 및 효율적 관리.

## 입력/출력 명세
### 입력
- Universe가 확정된 정규화 가격 데이터 (`adj_close`, `volume`, `amount`).
- 테마별 종목 맵핑 테이블 (`theme_map`).

### 출력
- **Stock Features**: 종목별 모멘텀, 변동성, 추세 지표.
- **Theme Features**: 테마 단위 집계 지표 (Weighted Return, Breadth, Concentration).

## 모듈 설계
### Stock Feature Module (최소 구현셋)
- **수익률(Returns)**: `ret_1d`, `ret_5d`, `ret_20d`, `ret_60d`, `ret_120d`.
- **변동성/리스크**: `vol_20d` (표준편차), `atr_14d`, `maxdd_60d`.
- **추세/반전**: MA Slope (20/60선 기울기), RSI, Z-Score.

### Theme Aggregate Module
- **Theme Return**: 테마 내 종목들의 수익률 집계 (동일 가중 또는 시총 가중).
- **Breadth (확산)**: 테마 내 상승 종목 비율 및 60일 신고가 달성 비율.
- **Concentration (쏠림)**: Herfindahl-Hirschman Index (HHI) 또는 상위 3개 종목 기여도.
- **Relative Strength**: 벤치마크(KOSPI/SPY) 대비 테마의 초과 수익률.

## 실무 포인트
- **Vectorization**: Pandas `rolling`, `shift` 연산을 활용한 고속 벡터 계산 수행.
- **Caching**: 계산 비용 절감을 위해 피처별 Parquet 캐싱 및 업데이트 전략 수립.
- **Scale Invariance**: 피처간 비교를 위한 정규화 (Z-Score 또는 Percentile) 처리 고려.

## 예시
### 테마 피처 산출 수식
- **Breadth(t)** = (상승 종목 수) / (테마 내 총 종목 수)
- **RS(t)** = (Theme\_Return\_20d) - (Benchmark\_Return\_20d)

### 코드 예시 (Vectorized Feature)
```python
def calculate_stock_features(df):
    """
    종목별 기초 팩터 산출 로직
    """
    # 수익률 계산
    df['ret_20d'] = df.groupby('ticker')['adj_close'].pct_change(20)
    # 변동성 계산
    df['vol_20d'] = df.groupby('ticker')['adj_close'].transform(lambda x: x.pct_change().rolling(20).std())
    return df
```
