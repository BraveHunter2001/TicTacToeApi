# TicTacToeApi
*Задание* : Спроектируйте и реализуйте REST API для игры в крестики нолики.  
*Стек*: ASP.NET, Entity Framework (Code-First), Postgresql, xUnit, Moq, Swagger

## Описание Api:
### Players
* **Create**  
[POST]  
Route: api/Players/Create  
Описание: Создает Игрока.  
Передать: Имя Игрока.  
Возвращает: Guid Игрока.  
* **Delete**  
[POST]  
Route: api/Players/Delete  
Описание: Удаляет Игрока.  
Передать: Guid Игрока.  
Возвращает: "Player %Имя игрока% was deleted"  
* **OwnerRooms**  
[GET]  
Route: api/Players/OwnerRooms  
Описание: Возвращает список guid комнат, которые игрок создавал.  
Передать: Guid Игрока.  
Возвращает: список guid комнат.  
* **GuestRooms**    
[GET]  
Route: api/Players/GuestRooms  
Описание: Возвращает список guid комнат, к которым игрок подключен.  
Передать: Guid Игрока.  
Возвращает: список guid комнат.  
### Rooms
* **Create**  
[GET]  
Route: api/Rooms/Create  
Описание: Создает комнату для игры в крестики нолики.  
Передать: Guid Игрока.  
Возвращает: guid комнаты.  
* **Delete**   
[POST]  
Route: api/Rooms/Delete  
Описание: Удаляет комнату  
Передать: Guid Игрока и guid комнаты.   
Возвращает: "Room %Guid комнаты% was delete."  
* **Connect**   
[POST]  
Route: api/Rooms/Connect  
Описание: Подключает игрока к комнате.  
Передать: Guid Игрока и Guid комнаты.  
Возвращает: "Connect is succsessfull"
* **Disconnect**   
[POST]  
Route: api/Rooms/Disconnect  
Описание: Отключает игрока от комнаты.  
Передать: Guid Игрока и Guid комнаты.  
Возвращает: "Disconnect is succsessfull"
* **InfoAbout**   
[GET]   
Route: api/Rooms/InfoAbout  
Описание: Возвращает информацию о команте: ее статус, Имя Создателя комнаты, Имя гостя, Имя победителя, Поле и Список ходов.  
Передать: Guid комнаты.  
Возвращает: Структуру InfoRoom  
* **MakeMove**   
[POST]  
Route: api/Rooms/MakeMove  
Описание: Делает ход в игре.  
Передать: Структуру Move  
Возвращает: Структуру MoveInfo  
### Описание структур:
* Move
  * idRoom - Guid - комнаты
  * idPlayer - Guid - Игрока
  * coords - координты (X - индекс строки, Y -Индекс колонки). Индекс начинается с нуля  

* InfoRoom  
  * status - Статус комнаты ( 0 - Ожидает подключения игрока, 1 - Идет игра.В данный момент можно делать ходы. 2 - Игра завершена)  
  * ownerName - Имя создателя комнаты.
  * guestName - Имя подключившегося к команте.
  * winnerName - Имя победителя в этой комнате.
  * field - Массив поля ( 0 - пусто, 1 - Первый игрок, 2 - Второй игрок)
  * moves - Список Ходов (координаты хода) 

* MoveInfo  
  * status - Статус комнаты ( 0 - Ожидает подключения игрока, 1 - Идет игра.В данный момент можно 
  * winnerName - Имя победителя в этой комнате.
  * field - Массив поля ( 0 - пусто, 1 - Первый игрок, 2 - Второй игрок)

