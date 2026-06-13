# FakeStore BDD API Tests

Небольшой BDD-фреймворк для API-тестирования сервиса [FakeStoreAPI](https://fakestoreapi.com/).

Цель проекта - показать, как можно организовать API-тесты через BDD-сценарии, разделить тестовую логику по слоям, добавить логирование, отчёты и запуск через GitHub Actions.

## Используемый стек

* C# / .NET 8
* Reqnroll
* NUnit
* RestSharp
* FluentAssertions
* Serilog
* Allure Report
* GitHub Actions

## Что проверяется

В проекте есть тесты для нескольких разделов FakeStoreAPI:

* Products
* Carts
* Users
* Auth

Покрыты сценарии получения данных, создания, обновления и удаления продукта, получения корзин, получения пользователей и авторизации.

## Структура проекта

```text
tests/FakeStore.BddTests
├── Clients              # API-клиенты
├── Config               # Настройки тестов
├── Context              # Контекст BDD-сценария
├── Features             # Gherkin-сценарии
├── Hooks                # Логирование до и после сценариев
├── Models               # Модели запросов
├── Reporting            # Mock-публикация отчёта
└── StepDefinitions      # Реализация шагов из feature-файлов
```

## Как запустить тесты локально

Запуск всех тестов:

```bash
dotnet test FakeStore.BddTests.sln
```

Запуск отдельных наборов тестов через теги:

```bash
dotnet test FakeStore.BddTests.sln --filter "TestCategory=smoke"
dotnet test FakeStore.BddTests.sln --filter "TestCategory=regression"
dotnet test FakeStore.BddTests.sln --filter "TestCategory=products"
dotnet test FakeStore.BddTests.sln --filter "TestCategory=carts"
dotnet test FakeStore.BddTests.sln --filter "TestCategory=users"
dotnet test FakeStore.BddTests.sln --filter "TestCategory=auth"
dotnet test FakeStore.BddTests.sln --filter "TestCategory=negative"
```

## Настройки

По умолчанию тесты используют:

```text
https://fakestoreapi.com
```

При необходимости базовый URL можно переопределить через переменную окружения:

```bash
FAKESTORE_BASE_URL=https://fakestoreapi.com
```

## Логирование

В проекте добавлено логирование через Serilog.

Логируются:

* начало и завершение тестового сценария;
* теги сценария;
* HTTP-метод и endpoint;
* статус ответа;
* время выполнения запроса;
* ошибка, если запрос завершился неуспешно.

Тела запросов и ответов не логируются по умолчанию, чтобы случайно не записывать чувствительные данные, например пароль или token.

## Allure Report

После запуска тестов формируются Allure results.

Для генерации HTML-отчёта нужен установленный Allure CLI.

Пример:

```bash
allure generate tests/FakeStore.BddTests/bin/Debug/net8.0/allure-results -o tests/FakeStore.BddTests/allure-report --clean
allure open tests/FakeStore.BddTests/allure-report
```

Папки `allure-results` и `allure-report` не добавляются в Git, потому что это сгенерированные файлы.

## Mock CDN publishing

Добавлен класс:

```text
Reporting/CdnReportPublisher.cs
```

Сейчас он работает как mock-реализация: проверяет наличие папки отчёта, считает файлы и логирует fake CDN URL. Реальную загрузку можно было бы добавить позже, заменив эту реализацию.

## GitHub Actions

В репозитории есть workflow:

```text
.github/workflows/api-tests.yml
```

Он выполняет восстановление зависимостей, сборку проекта и запуск тестов.

Так как FakeStoreAPI - внешний публичный сервис, в GitHub Actions он может возвращать `403 Forbidden` для GitHub runner. Локально при этом тесты могут проходить нормально.

Поэтому в workflow добавлена предварительная проверка доступности API. Если FakeStoreAPI доступен, тесты запускаются. Если сервис возвращает `403`, workflow сохраняет диагностику и пропускает внешние API-тесты, чтобы не считать проблемой фреймворка блокировку внешнего сервиса.

## Особенность FakeStoreAPI

FakeStoreAPI - демонстрационный сервис. Запросы `POST`, `PUT` и `DELETE` возвращают ответ, но не обязательно реально сохраняют изменения на сервере.

Поэтому тесты проверяют ответ API: статус, структуру JSON и ожидаемые поля, а не постоянное сохранение данных.