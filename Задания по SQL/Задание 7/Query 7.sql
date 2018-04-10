use Warehouse;
/*Задание 7*/
select m.First_Name as 'Имя', m.Last_Name 'Фамилия', Count(w.Manager_Id) as 'Количество складов'
from Managers m
inner join Warehouse_Manager w 
	on m.id = w.Manager_Id
GROUP BY m.First_Name, m.Last_Name
ORDER BY m.First_Name;
