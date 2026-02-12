# 07. Report & UI: 분석 결과 시각화 및 리포트 자동화

## 목적
생성된 신호와 분석 결과를 사용자가 이해하기 쉬운 형태(리포트/대시보드)로 요약하여 의사결정 지원.

## 입력/출력 명세
### 입력
- **Signal Results**: 당일 테마 랭킹, Exit 경고, 차기 후보 데이터.
- **Backtest Results**: 전략의 역사적 성과 데이터.
- **Visual Assets**: 수익률 곡선 그래프, 테마 맵 차트 등.

### 출력
- **Daily Markdown Report**: 핵심 신호가 요약된 텍스트 리포트.
- **REST API Response**: FastAPI를 통해 데이터 전달.
- **Interactive Dashboard**: Plotly 또는 Dash 기반 시각화 UI.

## 모듈 설계
### Report Generator
- **Template Engine**: Jinja2 활용 Markdown/HTML 템플릿 기반 자동 서식 작성.
- **Summary Logic**: 오늘 가장 강한 테마와 위험한 테마의 핵심 피처 요약.

### API Layer (FastAPI)
- `GET /signals/today`: 당일 주도 테마 및 신호 요약 조회.
- `GET /backtest/report`: 전략 누적 성과 및 주요 지표 조회.
- `GET /theme/{name}`: 특정 테마의 세부 구성 종목 및 피처 추이 조회.

## 실무 포인트
- **Evidence-based Layout**: 단순 랭킹 제시가 아닌, 왜 해당 테마가 선택되었는지(피처 근거)를 전면에 배치.
- **Alert System**: Exit 레벨 2 이상 발생 시 슬랙(Slack) 또는 텔레그램 알림 연동 고려.
- **Automation**: 매일 장 종료 후 데이터 인제스트부터 리포트 생성까지 배치(Batch) 프로세스 자동화.

## 예시
### 일간 리포트 템플릿 구조
```markdown
# [DATE] 테마 로테이션 데일리 리포트

## 1. 현재 주도 테마 (Top 3)
- **테마 A**: Leader Score [0.85], Breadth [90%], RS [1.2]
- **테마 B**: ...

## 2. 리스크 경고 (Exit Signals)
- **테마 C**: [Level 2] 주의 - Breadth 붕괴 포착.

## 3. 차기 유망 후보
- **테마 D**: 근거 - 최근 RS 개선세 뚜렷.
```

### API 응답 예시 (JSON)
```json
{
    "date": "2024-03-15",
    "market_regime": "Risk-On",
    "top_themes": [
        {"name": "반도체", "score": 0.92, "rank": 1},
        {"name": "2차전지", "score": 0.78, "rank": 2}
    ],
    "exit_alerts": [
        {"name": "초전도체", "level": 2, "reason": "Concentration Spike"}
    ]
}
```
