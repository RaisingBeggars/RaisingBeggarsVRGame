<p align="center">
  <img width="1149" height="361" src="https://github.com/user-attachments/assets/8b802787-7a5a-460f-b7a3-110c27aa3201" />
</p>

<div align="center">

**손짓 하나로 시작하는 VR 자수성가 시뮬레이션!!💵**  
**Inspired by "거지키우기"✨**

0원에서 시작해 구걸, 주식 투자, 상점 거래를 거쳐  
최종적으로 나만의 꿈의 섬을 얻는 **XR Hand Tracking 기반 VR 게임 프로젝트**입니다.

<br/>

![Unity](https://img.shields.io/badge/Unity-6000.2.2f1-000000?style=flat-square&logo=unity&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)
![XR Hands](https://img.shields.io/badge/XR%20Hands-1.6.1-6C63FF?style=flat-square)
![XR Interaction Toolkit](https://img.shields.io/badge/XR%20Interaction%20Toolkit-3.2.2-4285F4?style=flat-square)
![OpenXR](https://img.shields.io/badge/OpenXR-1.15.1-00599C?style=flat-square)
![Oculus XR](https://img.shields.io/badge/Oculus%20XR-4.5.2-1C1E20?style=flat-square)

</div>

---

## Overview

**RaisingBeggarsVRGame**은 XR Hand Tracking을 활용한 VR 경제 시뮬레이션 게임입니다.

플레이어는 0원에서 시작해 손동작으로 구걸하고, 실시간으로 변동되는 주식시장에 📈 **투자하기!!**    
1억 원을 모아 배를 구매하세요!!🛳️  배를 구매하면 새로운 ⛳️ **섬으로 이동**하며 게임을 **클리어**✨하게 됩니다! 

<img width="4418" height="1834" alt="image" src="https://github.com/user-attachments/assets/8b4e973a-9e26-4068-900b-24635b89440a" />


---

## Game Scenario

```text
0원에서 시작
   ↓
손동작으로 구걸
   ↓
주식 매수 / 매도
   ↓
자산 증식
   ↓
1억 원 달성
   ↓
배 구매
   ↓
섬 이동 및 클리어
```

---

## Gameplay

| 시스템 | 설명 |
|---|---|
| **구걸 시스템** | 특정 손동작을 인식하면 수익이 발생합니다. |
| **커스텀 제스처 인식** | XR Hand Tracking 기반 손 뼈대 데이터를 활용해 제스처를 판별합니다. |
| **주식 거래 시스템** | 주가가 일정 주기로 변동되며, 플레이어는 매수와 매도를 통해 자산을 불릴 수 있습니다. |
| **Pinch 상호작용** | 맵 이동, 주식 선택, 상품 구매 등 주요 조작에 사용됩니다. |
| **상점 거래** | 모은 돈으로 배를 구매하면 게임 클리어 조건을 달성합니다. |
| **엔딩 분기** | 자산 상태에 따라 클리어 엔딩 또는 실패 엔딩으로 전환됩니다. |

---

## Key Features

### 1. XR Hand Tracking 기반 제스처 인식

컨트롤러 없이 손동작만으로 게임을 진행할 수 있도록 XR Hand Tracking을 활용했습니다.

- 손 뼈대, Skeleton, 기반 추적
- 특정 제스처 인식 시 수익 발생
- 제스처 난이도와 종류에 따른 차등 보상 구조
- 직관적인 손 기반 VR 인터랙션

---

### 2. 실시간 주식 시뮬레이션

플레이어는 게임 내 주식시장에서 기업을 선택하고 매수/매도를 진행할 수 있습니다.

- 주가 30초 주기 갱신
- 주가 변동 카운트다운 표시
- 변동폭 제한을 통한 리스크 조절
- 키패드 기반 수량 입력
- 보유 주식 확인 및 매도 기능

---

### 3. 물리 기반 상점 상호작용

게임 클리어는 단순 버튼 클릭이 아니라, VR 환경에 맞춘 물리 상호작용으로 진행됩니다.

- 배 오브젝트를 Pinch로 집기
- 상자에 넣으면 구매 처리
- 충돌 감지 기반 구매 완료
- 클리어 씬 자동 전환

---

### 4. 엔딩 시스템

플레이어의 자산 상태와 구매 여부에 따라 엔딩이 달라집니다.

| 조건 | 결과 |
|---|---|
| 1억 원을 모아 배 구매 | 섬 이동, 클리어 엔딩 |
| 보유 자산이 마이너스 | 실패 엔딩 후 리셋 |

---

## Controls

| 조작 | 방법 |
|---|---|
| 맵 이동 | Pinch 제스처 |
| 구걸 | 숨겨진 손 제스처 수행 |
| 주식 선택 | Pinch로 주식 선택 |
| 주식 매수 | 키패드로 수량 입력 후 구매 |
| 주식 매도 | 보유 주식 선택 후 수량 입력 |
| 배 구매 | Pinch로 배를 집어 상자에 넣기 |

---

## Tech Stack

| Category | Stack |
|---|---|
| Engine | Unity 6000.2.2f1 |
| Language | C# |
| XR | Unity XR Hands |
| Interaction | XR Interaction Toolkit |
| Runtime | OpenXR, Oculus XR |
| Input | Unity Input System |
| Rendering | Universal Render Pipeline, URP |

---

## Project Structure

```text
RaisingBeggarsVRGame
├── Assets
│   └── Game assets, scripts, scenes, prefabs
├── Packages
│   └── Unity package dependencies
├── ProjectSettings
│   └── Unity project configuration
├── .vscode
│   └── Editor settings
└── README.md
```

---

## Getting Started

### 1. Clone Repository

```bash
git clone https://github.com/RaisingBeggars/RaisingBeggarsVRGame.git
cd RaisingBeggarsVRGame
```

### 2. Open Project

Unity Hub에서 프로젝트 폴더를 열어주세요.

권장 Unity 버전:

```text
Unity 6000.2.2f1
```

### 3. Package Import

프로젝트를 열면 `Packages/manifest.json` 기준으로 필요한 패키지가 자동으로 로드됩니다.

주요 패키지:

```text
com.unity.xr.hands
com.unity.xr.interaction.toolkit
com.unity.xr.management
com.unity.xr.oculus
com.unity.xr.openxr
com.unity.inputsystem
com.unity.render-pipelines.universal
```

### 4. Run

VR 디바이스 또는 XR 시뮬레이션 환경에서 실행합니다.  
Hand Tracking이 필요한 기능은 실제 XR Hand Tracking 지원 환경에서 테스트하는 것을 권장합니다.

---

## Demo Flow

```text
Start
  └─ 0원 상태로 게임 시작

Begging
  └─ 숨겨진 제스처를 찾아 수익 획득

Stock Trading
  ├─ 주식 선택
  ├─ 수량 입력
  ├─ 매수
  └─ 매도

Shop
  └─ 1억 원 달성 후 배 구매

Ending
  ├─ Clear: 섬 이동
  └─ Fail: 자산 마이너스 시 실패 엔딩
```

---

## Development Focus

이 프로젝트는 단순 VR 체험이 아니라, 손 기반 상호작용을 게임 경제 시스템과 연결하는 데 초점을 두었습니다.

- Hand Tracking 데이터를 게임 보상 로직과 연결
- VR 환경에 맞는 자연스러운 Pinch 인터랙션 설계
- 주식 가격 변동을 통한 리스크/보상 구조 구현
- 물리 충돌 기반 상점 구매 플로우 구현
- 자산 상태에 따른 씬 전환 및 엔딩 처리
