use Warehouse;
/*������� 9*/
select top 3 m.First_Name as '���', m.Last_Name as '�������', Sum(wp.Quantity_Product) as '���-�� ������ �� �������'
from Managers m
inner join  Warehouse_Manager wm 
		on wm.Manager_Id = m.id
inner join Warehouse_Products wp 
		on wp.Warehouse_Id = wm.Warehouse_Id
GROUP BY m.First_Name, m.Last_Name
ORDER BY Sum(wp.Quantity_Product) DESC;