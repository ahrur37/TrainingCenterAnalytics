## Требования 
- macOS (Apple Silicon)
- Docker Desktop (только для версии Self-hosted)

## Установка и запуск (Self-hosted)
1. Локально развернуть API и БД
```bash
git clone https://github.com/ahrur37/TrainingCenterAnalytics
cd TrainingCenterAnalytics
docker compose -f docker-compose.local.yml up --build -d
```
2. Скачать архив `selfhosted.zip` из [последнего релиза](https://github.com/ahrur37/TrainingCenterAnalytics/releases/latest)
3. Распаковать архив
4. Запустить в терминале:
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
