# 04. Regime Model: 시장 국면 분류 및 매크로 지표

## 목적
시장 전체의 리스크 상태를 정의하여 테마 로테이션 전략의 노출 비중과 자산 배분 비중 결정.

## 입력/출력 명세
### 입력
- **Index Data**: KOSPI, KOSDAQ, S&P500, NASDAQ 지수 데이터.
- **Macro Indicators**: VIX(변동성), 미국채 10년물 금리, 달러 인덱스 (DXY), 하이일드 스프레드.

### 출력
- **Regime Score**: 시장 리스크 점수 (0~1 범위).
- **Market State**: `Risk-On`, `Risk-Off`, `Sideways`, `Vol-High` 등의 레이블링.

## 모듈 설계
### Macro Signal Module
- **Trend Filter**: 지수 200일 이동평균선 상단 여부 및 60일 이동평균 기울기 산출.
- **Volatility Filter**: VIX 지수의 절대 수준 및 최근 변화율(ROC) 측정.
- **Rate Sensitivity**: 금리 변화에 따른 성장주/가치주 유리 환경 분석.

### Classification Engine
- **Rule-based**: 지수 추세와 변동성을 조합한 불 논리(Boolean Logic) 분류.
- **Statistical Model**: 최근 변동성 국면(HMM 등) 기반 상태 정의 (선택적).

## 실무 포인트
- **False Signals**: 지수 횡보장에서의 잦은 국면 전환(Whipsaw) 방지를 위한 필터 적용.
- **Lagging Effect**: 매크로 지표는 가격에 후행하거나 노이즈가 많으므로 평활화(Smoothing) 처리 필수.
- **Regime-Specific Strategy**: 국면별로 유효한 테마 특성 (성장/방어/고배당 등) 매핑.

## 예시
### 국면 분류 규칙 (Decision Tree)
1. **지수 > 200MA** AND **VIX < 20** : `Risk-On (Bullish)`
2. **지수 < 200MA** OR **VIX > 25** : `Risk-Off (Bearish)`
3. **지수 근처 횡보** AND **Low Vol** : `Regime Transition / Sideways`

### 데이터 스키마
```python
regime_schema = {
    'date': 'datetime64',
    'market_trend': 'float',  # 지수 대비 이격도
    'vix_level': 'float',     # 변동성 수준
    'regime_label': 'string', # 'Risk-On' | 'Risk-Off'
    'active_weight': 'float'  # 권장 노출 비중 (0.0 ~ 1.0)
}
```
