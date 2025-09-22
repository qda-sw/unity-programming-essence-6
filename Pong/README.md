# 🏓 Network Pong Game

Unity로 구현된 네트워크 멀티플레이 퐁 게임입니다.

## 🎮 게임 특징

- **네트워크 멀티플레이**: Unity Netcode for GameObjects 사용
- **실시간 동기화**: 패들 움직임과 공 물리 동기화
- **간단한 조작**: 키보드를 사용한 패들 제어
- **점수 시스템**: 먼저 5점을 얻는 플레이어가 승리

## 🕹️ 조작법

### Player 1 (왼쪽 패들)
- **W**: 위로 이동
- **S**: 아래로 이동

### Player 2 (오른쪽 패들)
- **↑**: 위로 이동
- **↓**: 아래로 이동

## 🚀 실행 방법

1. **Unity 6000.0.58f1** 이상에서 프로젝트 열기
2. **MenuScene**에서 게임 시작
3. **Host Game** 또는 **Join Game** 선택
4. 게임 플레이!

## 📁 프로젝트 구조

```
Assets/
├── Scripts/
│   ├── NetworkManagerUI.cs      # 네트워크 연결 UI 관리
│   ├── PaddleController.cs      # 패들 움직임 제어
│   ├── BallController.cs        # 공 물리 및 동기화
│   ├── PongGameManager.cs       # 게임 상태 및 점수 관리
│   └── PlayerSpawner.cs         # 플레이어 스폰 관리
├── Scenes/
│   ├── MenuScene.unity          # 메뉴/로비 씬
│   └── GameScene.unity          # 게임 플레이 씬
└── Prefabs/                     # 게임 오브젝트 프리팹들
```

## 🔧 기술 스택

- **Unity 6000.0.58f1**
- **Unity Netcode for GameObjects 1.8.1**
- **C#**
- **2D Physics**

## 📚 학습 목표

이 프로젝트는 다음을 학습합니다:

- **네트워크 프로그래밍 기초**
- **Unity Netcode for GameObjects 사용법**
- **실시간 멀티플레이 게임 동기화**
- **클라이언트-서버 아키텍처**
- **네트워크 변수와 RPC 사용**

---

*이 프로젝트는 『유니티 프로그래밍 에센스 6』 7부를 기반으로 구현되었습니다.*