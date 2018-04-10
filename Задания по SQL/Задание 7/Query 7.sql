use Warehouse;
/*������� 7*/
select m.First_Name as '���', m.Last_Name '�������', Count(w.Manager_Id) as '���������� �������'
from Managers m
inner join Warehouse_Manager w 
	on m.id = w.Manager_Id
GROUP BY m.First_Name, m.Last_Name
ORDER BY m.First_Name;
