## Трекер проектов

## Для запуска

```
docker compose --env-file .env up
```
Основной: `localhost:5000/swagger`

Сервис истории: `localhost:5001/swagger`

## Чек лист: [Файл](./docs/checklist.md)

## Postman коллекции

- [Main Api](./docs/ProjectTracker.Api.postman_collection.json)

- [History Api](./docs/ProjectTracker.History.Api.postman_collection.json)

## Пометки:

- `Docker.minio-setup` и `minio-setup.sh` `minio-policy.json` служит только для демонтрации, чтобы хранилище создалось сразу со всем нужным для работы приложения

## Проблемы которые могут возникнуть:

- Получение файла отчета по ссылке может не работать из-за прокси (я его просто выключил)