# 🏓 Network Pong Game - Development Summary

## 📋 프로젝트 개요

Unity 6와 Unity Netcode for GameObjects를 사용하여 구현된 멀티플레이어 퐁 게임입니다. 
『유니티 프로그래밍 에센스 6』 7부(18-20장)의 네트워크 게임 개발 내용을 바탕으로 제작되었습니다.

## 🏗️ 아키텍처 설계

### 네트워크 아키텍처
- **클라이언트-서버 모델**: Unity Netcode for GameObjects 사용
- **권한 분리**: 
  - 서버(Host): 공 물리, 점수 관리, 게임 상태
  - 클라이언트: 패들 입력, UI 표시

### 핵심 컴포넌트

#### 1. NetworkManagerUI.cs
- **역할**: 네트워크 연결 및 로비 UI 관리
- **기능**: Host/Join 버튼, 연결 상태 표시
- **특징**: 씬 전환과 연결 관리 통합

#### 2. PaddleController.cs  
- **역할**: 패들 움직임 및 네트워크 동기화
- **기능**: 키보드 입력 처리, 위치 동기화
- **특징**: 소유권 기반 입력 처리, 경계 제한

#### 3. BallController.cs
- **역할**: 공 물리 및 네트워크 동기화  
- **기능**: 물리 시뮬레이션, 충돌 감지, 위치/속도 동기화
- **특징**: 서버 권한 물리, NetworkVariable 사용

#### 4. PongGameManager.cs
- **역할**: 게임 상태 및 점수 관리
- **기능**: 점수 추적, 승리 조건, 게임 재시작
- **특징**: 싱글톤 패턴, 네트워크 점수 동기화

#### 5. PlayerSpawner.cs
- **역할**: 플레이어 연결 및 패들 할당
- **기능**: 자동 패들 생성, 소유권 할당, 입력키 설정
- **특징**: 연결/해제 이벤트 처리, 최대 2명 제한

#### 6. BoundarySetup.cs
- **역할**: 게임 경계 및 골대 생성
- **기능**: 동적 경계 생성, 충돌 영역 설정
- **특징**: 2D 충돌체 사용, 에디터 기즈모 지원

## 🔧 기술적 구현 특징

### 네트워크 동기화
```csharp
// NetworkVariable을 통한 실시간 동기화
private NetworkVariable<Vector2> networkPosition = new NetworkVariable<Vector2>();
private NetworkVariable<int> player1Score = new NetworkVariable<int>(0);

// RPC를 통한 이벤트 전달
[ServerRpc]
public void UpdatePositionServerRpc(Vector3 position)

[ClientRpc]  
private void UpdatePositionClientRpc(Vector3 position)
```

### 권한 분리 패턴
```csharp
// 서버에서만 처리
if (!IsServer) return;

// 소유자만 입력 처리
if (!IsOwner) return;
```

### 이벤트 기반 아키텍처
```csharp
// 네트워크 변수 변경 이벤트
networkPosition.OnValueChanged += OnPositionChanged;
player1Score.OnValueChanged += OnPlayer1ScoreChanged;
```

## 🎮 게임플레이 메커니즘

### 플레이어 제어
- **Player 1**: W/S 키로 좌측 패들 제어
- **Player 2**: ↑/↓ 키로 우측 패들 제어
- **움직임 제한**: Y축 범위 내에서만 이동 가능

### 공 물리
- **초기 발사**: 랜덤 방향으로 시작
- **속도 증가**: 패들과 충돌 시 점진적 가속
- **최대 속도**: 제한된 최대 속도로 게임 밸런스 유지
- **벽 반사**: 상하 벽에서 반사

### 점수 시스템
- **득점 조건**: 상대편 골대에 공이 들어갈 때
- **승리 조건**: 먼저 5점에 도달하는 플레이어
- **공 리셋**: 득점 후 1초 지연 후 중앙에서 재시작

## 📁 프로젝트 구조

```
Pong/
├── Assets/
│   ├── Scripts/                 # C# 스크립트
│   │   ├── NetworkManagerUI.cs   # 네트워크 UI
│   │   ├── PaddleController.cs   # 패들 제어
│   │   ├── BallController.cs     # 공 물리
│   │   ├── PongGameManager.cs    # 게임 관리
│   │   ├── PlayerSpawner.cs      # 플레이어 생성
│   │   └── BoundarySetup.cs      # 경계 설정
│   ├── Scenes/                  # Unity 씬
│   │   ├── MenuScene.unity       # 메뉴/로비
│   │   └── GameScene.unity       # 게임플레이
│   ├── Prefabs/                 # 프리팹
│   │   ├── Paddle.prefab         # 패들 프리팹
│   │   └── Ball.prefab           # 공 프리팹
│   └── Materials/               # 머티리얼
│       ├── PaddleMaterial.mat    # 패들 머티리얼
│       └── BallMaterial.mat      # 공 머티리얼
├── ProjectSettings/             # 프로젝트 설정
├── README.md                    # 프로젝트 설명
└── BUILD.md                     # 빌드 가이드
```

## 🚀 배포 및 테스트

### 로컬 테스트
1. Unity에서 Build and Run
2. 실행파일 2개 실행
3. Host/Join으로 연결 테스트

### 네트워크 테스트  
1. 서로 다른 컴퓨터에 배포
2. 네트워크 연결 확인
3. 지연시간 및 동기화 테스트

## 🎯 학습 성과

### 네트워크 프로그래밍
- ✅ 클라이언트-서버 아키텍처 이해
- ✅ 권한 분리 패턴 적용
- ✅ 네트워크 변수와 RPC 활용
- ✅ 소유권 기반 제어 구현

### Unity 개발
- ✅ NetworkBehaviour 상속 구조
- ✅ 2D 물리 시스템 활용
- ✅ 프리팹 기반 오브젝트 관리
- ✅ 씬 관리 및 UI 통합

### 게임 개발
- ✅ 실시간 멀티플레이 게임 설계
- ✅ 게임 상태 관리 패턴
- ✅ 입력 처리 및 물리 시뮬레이션
- ✅ 밸런싱 및 사용자 경험 고려

## 📈 향후 개선 사항

### 기능 확장
- [ ] 관전자 모드 추가
- [ ] 플레이어 닉네임 시스템
- [ ] 게임 결과 저장/통계
- [ ] 다양한 게임 모드

### 기술적 개선
- [ ] 네트워크 지연 보상
- [ ] 재연결 기능
- [ ] 서버 선택 UI
- [ ] 음향 효과 추가

---

*이 프로젝트는 Unity 네트워크 게임 개발 학습을 위해 제작되었습니다.*