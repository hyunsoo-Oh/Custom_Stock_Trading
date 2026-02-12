# 01. Data Ingest: 시장 데이터 수집 및 정합성 관리

## 목적
한국(KRX) 및 미국(US) 시장의 가격, 거래량, 시가총액 데이터를 안정적으로 수집하고 백테스트 및 실전 분석을 위한 시점 정합성 보장.

## 입력/출력 명세
### 입력 (External Sources)
- **한국**: FinanceDataReader (KRX, NAVER), 증권사 API (선택적).
- **미국**: yfinance (Yahoo Finance), Polygon.io (유료 확장 가능).
- **항목**: 일봉 OHLCV, 시가총액, 거래대금, 수정 계수 (Adjustment Factor).

### 출력 (Data Schema)
- **형식**: Parquet (Ticker별 분할 저장 권장).
- **컬럼**: `date` (datetime64), `ticker` (str), `open`, `high`, `low`, `close`, `adj_close`, `volume`, `amount`, `market_cap`.

## 모듈 설계
### Data Collector Module
- **KRX Fetcher**: FinanceDataReader 활용 상장 종목 리스트 및 일봉 데이터 수집.
- **US Fetcher**: yfinance 활용 S&P500/Nasdaq 위주 데이터 수집 및 수정 주가 추출.
- **Ingestion Manager**: API 호출 제한(Rate Limit) 관리 및 실패 시 재시도 로직 포함.

### Consistency Manager
- **Time Checker**: 미래 데이터 유입 방지를 위한 시점 검증 수행.
- **Adj Price Processor**: 배당, 분할 발생 시 전체 과거 데이터 재동기화 또는 점진적 업데이트 처리.

## 실무 포인트
- **수정 주가(Adj Close)**: 단순 Close가 아닌 자산 배분 및 수익률 계산을 위한 수정 주가 활용 필수.
- **Corporate Action**: 유상증자, 감자 등 이벤트 발생 시 가격 불연속성 제거를 위한 보정 로직 적용.
- **저장 구조**: 대용량 처리를 위해 `date`와 `ticker` 기반 인덱싱 및 Parquet 압축 활용.

## 예시
### 데이터 스키마 (SQL/Parquet)
```python
# Raw Market Data 저장 예시
raw_data_schema = {
    'date': 'datetime64[ns]', # 거래 날짜
    'ticker': 'string',       # 종목 코드 (KOSPI: 005930, US: AAPL)
    'open': 'float64',        # 시가
    'close': 'float64',       # 종가
    'adj_close': 'float64',   # 수정 종가 (핵심)
    'volume': 'int64',        # 거래량
    'amount': 'int64',        # 거래 대금
    'market_cap': 'int64'     # 당일 시가총액
}
```

### 처리 로직 예시
```python
import FinanceDataReader as fdr

def ingest_krx_data(ticker, start_date, end_date):
    """
    FinanceDataReader 활용 한국 주식 수집 처리
    """
    df = fdr.DataReader(ticker, start_date, end_date)
    # 데이터 정규화 및 수정 주가 확인 로직 포함
    return df
```
