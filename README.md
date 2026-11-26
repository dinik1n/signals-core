# signals-core

Библиотека для обработки одноканальных сигналов. Не зависит от интерфейса.

## Проекты
- `SignalProcessing.Core` — библиотека
- `SignalProcessing.Core.Tests` — модульные тесты (xUnit)

Требуется .NET Framework 4.8. Библиотека собирается сама, без приложения (`SignalProcessing.Core.sln`).

## Связанные репозитории
Приложение, которое использует библиотеку:
https://github.com/dinik1n/signals-generator-app

Оба репозитория клонируются в одну папку, рядом друг с другом:

```
D:\<рабочий каталог>\
├── signals-core\
└── signals-generator-app\
```

`SignalGenerator.sln` ссылается на проект библиотеки по относительному пути
`..\signals-core\SignalProcessing.Core\`, поэтому имена и расположение папок менять нельзя.

## Состав библиотеки

| Класс | Что делает | Исключения |
|---|---|---|
| `SingleChannelSignal` | Сигнал: значения и частота | `ArgumentNullException` — data null; `ArgumentOutOfRangeException` — частота ≤ 0 |
| `SingleChannelSignalWriter` | Запись сигнала в файл | `ArgumentNullException` — signal или путь null |
| `SingleChannelSignalReader` | Чтение сигнала из файла | `InvalidDataException` — файл пуст, строка не число, частота ≤ 0 |
| `OfflineFilter` | Скользящее среднее по всему сигналу | `ArgumentNullException` — data null; `ArgumentOutOfRangeException` — окно ≤ 0 или чётное |
| `OnlineFilter` | Скользящее среднее по частям, метод `Reset` | `ArgumentOutOfRangeException` — окно ≤ 0; `ArgumentNullException` — data null |
| `OfflineMedianFilter` | Медианный фильтр по всему сигналу | как у `OfflineFilter` |
| `OnlineMedianFilter` | Медианный фильтр по частям, метод `Reset` | `ArgumentOutOfRangeException` — окно ≤ 0 или чётное; `ArgumentNullException` — data null |
| `ExponentialSmoothingFilter` | Экспоненциальное сглаживание, метод `Reset` | `ArgumentOutOfRangeException` — alpha вне (0; 1]; `ArgumentNullException` — data null |
| `SignalOperations` | `Sum` — сумма сигналов, `Concat` — склейка | `ArgumentNullException` — список или элемент null; `ArgumentException` — список пуст или частоты различаются |
| `Resampler` | Смена частоты сигнала | `ArgumentNullException` — signal null; `ArgumentOutOfRangeException` — частота ≤ 0 |
| `CircularBuffer<T>` | Циклический буфер фиксированной ёмкости | `ArgumentOutOfRangeException` — ёмкость ≤ 0 или неверный индекс |
| `SingleChannelRangeAnalyzer` | `<что делает>` | `<исключения>` |

## Запуск тестов
Нужны пакеты NuGet: `xunit` (2.x), `xunit.runner.visualstudio`, `Microsoft.NET.Test.Sdk`
(устанавливаются автоматически при сборке).

1. Откройте `SignalProcessing.Core.sln` в Visual Studio.
2. Build → Build Solution.
3. Test → Test Explorer → Run All Tests.

Зелёные тесты — прошли, красные — не прошли.