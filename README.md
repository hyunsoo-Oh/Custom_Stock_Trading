# Custom Stock Trading: 테마 로테이션 분석 시스템 (MVP)

본 시스템은 한국(KRX)과 미국(US) 시장의 **테마 로테이션(Theme Rotation)**을 분석하여, 

현재 시장 주도 테마 정의, 하락 전조 감지 및 차기 후보를 발굴하는 퀀트 엔진

## 🎯 핵심 아키텍처 목표 (The 4 Pillars)
시스템은 매 시점(t)마다 다음 네 가지 핵심 결과물을 산출

1.  **ThemeScoreboard(t)**: 테마별 상대강도, 모멘텀, 확산(Breadth), 변동성/리스크 점수 관리.
2.  **Regime(t)**: 현재 시장 국면 (리스크온/오프, 성장/가치, 금리 민감도 등) 분류.
3.  **ExitSignal(leader_theme, t)**: 리더 테마의 경고 및 청산 신호 (확률/스코어 기반) 생성.
4.  **NextCandidates(t)**: "다음 주도 테마 Top-N" 및 선정 근거 피처 도출.

---

## 🛠 시스템 파이프라인 (7-Step)

### 1. Data Ingest
- **한국**: KRX, 네이버, FinanceDataReader 기반 일봉 OHLCV + 시총 + 거래대금 수집.
- **미국**: yfinance, Polygon 기반 동일 스펙 데이터 인제스트.
- **공통**: 분할/배당 반영된 **Adj Price** 필수 관리.

### 2. Normalization & Universe
- **필터링**: 거래대금 상위 N%, 시가총액 하위 Cut-off, 상장 기간(예: 1년 이상) 적용.
- **생존 편향 제거**: 과거 시점의 상장 폐지 종목 포함을 통한 백테스트 신뢰도 확보.

### 3. Feature Store
- **종목 피처**: 수익률(1D/5D/20D/60D/120D), 변동성(ATR, Vol), 유동성(회전율) 계산.
- **테마 피처(Agg)**: 
    - **Breadth**: 테마 내 상승 종목 비율 및 신고가 비율 산출.
    - **Concentration**: 상위 3개 종목의 기여도 쏠림 측정.
    - **Relative Strength**: KOSPI/SPY 대비 초과 수익 성능 분석.

### 4. Regime Model
- **입력 지표**: 지수 이동평균(200MA), VIX, 미국채 금리, 달러 인덱스 활용.
- **출력**: Risk-On/Off, High/Low Vol, Trend/Range 국면 정의.

### 5. Signal Engine
- **Leader Detection**: 상대강도 + Breadth - Concentration Penalty 알고리즘 적용.
- **Exit Logic**: Breadth 붕괴, 쏠림 심화, 추세 훼손 시 경고 신호 발생.
- **Next Ranking**: 역사적 로테이션 확률 및 국면별 최적 테마 랭킹 산출.

### 6. Backtest & Evaluation
- **현실적 검증**: 수수료 및 슬리피지 모델 포함.
- **워크포워드**: Rolling window 기반 검증을 통한 과최적화 방지.
- **성과 지표**: CAGR, Sharpe, MDD, Turnover, Hit Rate 도출.

### 7. Report & UI
- **Daily Report**: 현재 Top 테마, Exit 경고 레벨, 다음 후보 Top-N 및 근거 피처 요약 출력.

---

## 🛡 데이터 원칙 (Fail-safe)
1.  **시점 정합성(Time Consistency)**: 미래 정보 활용 방지를 위한 `effective_from/to` 관리 철저.
2.  **편향 제거**: 생존 편향 및 유동성 편향 엄격 통제.
3.  **수정 주가 우선**: 모든 수익률 계산 시 Corporate Action 반영 가격 사용 원칙.

## 📂 프로젝트 구조
- `/src`: 데이터 파이프라인 및 엔진 소스 코드
- `/docs`: 각 스텝별 세부 설계 명세서
- `/data`: Parquet 시계열 데이터 및 SQLite 메타데이터
- `/config`: 유니버스 필터 및 전략 파라미터 설정
