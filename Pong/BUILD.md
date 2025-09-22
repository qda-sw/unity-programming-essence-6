# 🏓 Network Pong - Build Instructions

## 🛠️ 빌드 방법

### 1. Unity에서 빌드하기

1. **Unity Hub**에서 프로젝트 열기
   - Unity 6000.0.58f1 LTS 버전 사용
   - Pong 폴더를 Unity 프로젝트로 열기

2. **씬 확인**
   - File > Build Settings
   - Build에 포함된 씬들:
     - `Assets/Scenes/MenuScene.unity` (Index: 0)
     - `Assets/Scenes/GameScene.unity` (Index: 1)

3. **빌드 설정**
   - File > Build Settings > Player Settings
   - Company Name과 Product Name 설정
   - Platform 선택 (Windows, Mac, Linux)

4. **빌드 실행**
   - Build and Run 또는 Build 선택
   - 빌드 폴더 지정 후 실행

### 2. 멀티플레이 테스트

#### 로컬 테스트 (같은 컴퓨터)
1. 빌드된 실행파일을 **2개 실행**
2. 첫 번째 플레이어: **Host Game** 클릭
3. 두 번째 플레이어: **Join Game** 클릭
4. 게임 시작!

#### 네트워크 테스트 (다른 컴퓨터)
1. 각 컴퓨터에 빌드 파일 배포
2. 호스트 컴퓨터에서 **Host Game** 클릭
3. 클라이언트 컴퓨터에서 **Join Game** 클릭
4. 네트워크 연결 확인 후 게임 플레이

### 3. 조작법

| 플레이어 | 위로 이동 | 아래로 이동 |
|----------|-----------|-------------|
| Player 1 (좌측) | W | S |
| Player 2 (우측) | ↑ | ↓ |

### 4. 게임 규칙

- **승리 조건**: 먼저 5점을 얻는 플레이어가 승리
- **득점**: 상대방 골대에 공이 들어가면 1점
- **공 리셋**: 득점 후 1초 뒤 중앙에서 랜덤 방향으로 재시작

### 5. 문제 해결

#### 연결이 안 될 때
- 방화벽 설정 확인
- 네트워크 연결 상태 확인
- Unity Transport 설정 확인

#### 게임이 느릴 때
- 네트워크 지연 확인
- 프레임레이트 설정 확인 (30 FPS 권장)

#### 동기화 문제
- 서버(Host) 권한으로 공 물리 처리
- 네트워크 변수로 점수/게임상태 동기화

### 6. 개발자 정보

- **Unity 버전**: 6000.0.58f1 LTS
- **네트워킹**: Unity Netcode for GameObjects 1.8.1
- **플랫폼**: Windows, macOS, Linux
- **최대 플레이어**: 2명

---

*이 게임은 학습 목적으로 제작되었습니다.*