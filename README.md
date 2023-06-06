Поднять контейнеры с бд


` docker compose up -d oms-auth-db oms-db `


Поднять контейнеры с микросервисами


` docker compose up -d oms-auth oms `


Открыть swagger сервиса аутентификации: http://localhost:5030/swagger/index.html


Открыть swagger сервиса заказов: http://localhost:5031/swagger/index.html
