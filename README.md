## Требования 
- macOS (Apple Silicon)
- Docker Desktop (только для версии Self-hosted)

## Установка и запуск (Self-hosted)
1. Склонировать репозиторий. Запустить в терминале:
```bash
git clone https://github.com/ahrur37/TrainingCenterAnalytics
cd TrainingCenterAnalytics
docker compose -f docker-compose.local.yml up --build -d
```
2. Создать пароль и JWT-токен. Запустить в терминале:
```bash
cp .env.example .env
```
Заполнить значения в .env.
4. Локально развернуть API и БД
```bash
docker compose -f docker-compose.local.yml up --build -d
```
4. Скачать архив `selfhosted.zip` из [последнего релиза](https://github.com/ahrur37/TrainingCenterAnalytics/releases/latest)
5. Распаковать архив
6. Запустить в терминале:
```bash
xattr -cr selfhosted/
cd selfhosted/                                                                                                                      
./selfhosted
```
## Установка и запуск (Cloud)
1. Скачать архив `cloud.zip` из [последнего релиза](https://github.com/ahrur37/TrainingCenterAnalytics/releases/latest)
2. Распаковать архив
3. Запустить в терминале:
```bash
xattr -cr cloud/
cd cloud/                                                                                                                      
./cloud
```
