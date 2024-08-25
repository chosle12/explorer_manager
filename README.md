# ExplorerProcessManager

`ExplorerProcessManager`는 Windows에서 `explorer.exe` 프로세스의 실행을 관리하기 위해 설계된 C# 유틸리티입니다. `explorer.exe`는 고유한 실행 특성 때문에 .NET의 `Process` 클래스를 사용하여 실행할 경우, 새로 생성된 프로세스를 추적하고 관리하기 어려운 점이 있습니다. 이 프로젝트는 이러한 문제를 해결하기 위해 필요한 로직을 캡슐화한 클래스를 제공합니다.

## 주요 기능

- **Explorer 프로세스 관리**: `explorer.exe` 프로세스를 쉽게 실행하고 관리할 수 있습니다.
- **새로 생성된 프로세스 추적**: 실행된 특정 `explorer.exe` 인스턴스를 식별하고 관리할 수 있는 기능을 제공합니다.
- **간단한 API**: 기존 .NET 애플리케이션에 쉽게 통합할 수 있는 직관적인 클래스 인터페이스를 제공합니다.

## 솔루션에 포함된 프로젝트

이 저장소에는 다음과 같은 프로젝트가 포함되어 있습니다:

1. **ExplorerProcess**: `ExplorerProcessManager` 클래스의 핵심 기능을 포함한 프로젝트로, `explorer.exe` 프로세스를 관리하는 주요 로직을 제공합니다.

2. **ExplorerForm**: `ExplorerProcessManager`를 Windows Forms 애플리케이션에서 사용하는 방법을 보여주는 샘플 프로젝트입니다. 이 프로젝트는 그래픽 인터페이스에서 클래스를 어떻게 통합할 수 있는지 설명합니다.

3. **ExplorerNotUI**: UI가 없는 콘솔 애플리케이션에서 `ExplorerProcessManager`를 사용하는 방법을 보여주는 샘플 프로젝트입니다. 이 프로젝트는 백엔드 또는 서비스 지향 애플리케이션에서 클래스를 활용하는 방법을 설명합니다.

## 시작하기

### 필요 조건

- [.NET SDK](https://dotnet.microsoft.com/download) (버전 6.0 이상)
- [Visual Studio](https://visualstudio.microsoft.com/) (2019 버전 이상 권장)

### 솔루션 빌드하기

1. 리포지토리를 로컬 머신에 클론합니다:
   ```bash
   git clone https://github.com/yourusername/ExplorerProcessManager.git
2. Visual Studio에서 ExplorerManager.sln 솔루션 파일을 엽니다.

3. Build > Build Solution을 선택하거나 Ctrl+Shift+B를 눌러 솔루션을 빌드합니다.

### 예제 실행하기
- ExplorerForm: ExplorerForm을 시작 프로젝트로 설정하고 실행합니다. 이 작업을 통해 ExplorerProcessManager를 사용하는 Windows Forms 애플리케이션이 실행됩니다.

- ExplorerNotUI: ExplorerNotUI를 시작 프로젝트로 설정하고 실행합니다. 이 작업을 통해 ExplorerProcessManager를 사용하는 콘솔 애플리케이션이 실행됩니다.

## 사용법
- ExplorerProcessManager를 여러분의 프로젝트에서 사용하려면 ExplorerProcess 프로젝트를 솔루션에 포함시키거나, 해당 클래스 파일들을 직접 참조할 수 있습니다.

## 라이선스
이 프로젝트는 MIT 라이선스 하에 라이선스됩니다. 자세한 내용은 LICENSE 파일을 참조하십시오.