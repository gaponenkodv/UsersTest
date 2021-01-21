Разворачивание приложения
```
dotnet restore
```
В appsettings.json добавить путь и доступ к базе pgsql

Запустить миграции
```
dotnet ef database update
```
Запуск серверв
```
dotnet run
```

Перейти в папку фронта
```
cd ng-test
npm i
ng serve
```

фронт доступен на 
```
localhost:4200
```
бэк на 
```
https://localhost:5001
```
для апи есть swagger
```
https://localhost:5001/swagger/index.html
```
