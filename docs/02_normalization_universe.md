# 02. Normalization & Universe: 테마 분석 유니버스 확정

## 목적
노이즈(저유동성, 소형주) 제거 및 분석 데이터의 정규화 작업을 통해 테마 로테이션 분석의 유효성 확보.

## 입력/출력 명세
### 입력
- Ingest 단계에서 생성된 Raw Market Data (Parquet).
- 상장 폐지 종목 리스트 및 과거 시점의 테마 맵핑 정보.

### 출력
- **Cleaned Universe**: 분석 대상 종목 리스트 (Ticker & 기간 정의).
- **Normalized Data**: 이상치(Outlier) 제거 및 스케일링이 완료된 가격 데이터.

## 모듈 설계
### Universe Filter Module
- **Liquidity Filter**: 최근 N일 평균 거래 대금 하위 M% 제거.
- **Cap Filter**: 시가총액 기반 분석 대상 범위 설정 (예: 대형주/중형주/소형주 구분).
- **Listing Period Filter**: 상장 후 최소 기간 (예: 250거래일) 미만 종목 배제.

### Bias Removal Module
- **Survivorship Bias Fix**: 과거 특정 시점에 상장되어 있었으나 현재 폐지된 종목을 포함한 유니버스 구성.
- **Effective Date Management**: 테마 구성 종목의 시간에 따른 변화 추적.

## 실무 포인트
- **Look-ahead Bias**: 미래의 시총/거래대금 정보를 사용하여 현재 유니버스를 결정하지 않도록 설계.
- **Outlier Handling**: 비정상적 주가 폭등/폭락 또는 데이터 오류에 대한 클렌징 로직 포함.
- **Rebalancing Cycle**: 유니버스 갱신 주기 (매주/매월) 정의를 통한 턴오버 제어.

## 예시
### 유니버스 필터 조건 (Config)
```json
{
    "universe_config": {
        "min_avg_amount": 1000000000, // 일평균 거래대금 10억 이상
        "min_market_cap": 50000000000, // 시가총액 500억 이상
        "min_history_days": 250,       // 최소 상장 기간
        "include_delisted": true       // 상장 폐지 종목 포함 여부
    }
}
```

### 유니버스 생성 쿼리 (SQL)
```sql
/* 과거 시점 t의 유니버스 산출 예시 */
SELECT ticker
FROM daily_market_data
WHERE date = :target_date
  AND avg_amount_20d >= :threshold_amount
  AND market_cap >= :threshold_cap
```
