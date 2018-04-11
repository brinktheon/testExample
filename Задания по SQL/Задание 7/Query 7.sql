use Warehouse;
select m.First_Name as 'Имя', m.Last_Name 'Фамилия', 
		(
			Select Count(w.Manager_Id) as 'Количество складов'
			from Warehouse_Manager w
			Where m.id = w.Manager_Id
		 )	
from Managers m;