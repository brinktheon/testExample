use Warehouse;
/*������� 9*/
select top 3 m.First_Name as '���', m.Last_Name as '�������',
			 (
					select Sum(wp.Quantity_Product) as '���-�� ������ �� �������'
					from Warehouse_Manager wm
					inner join Warehouse_Products wp 
						on wp.Warehouse_Id = wm.Warehouse_Id AND wm.Manager_Id = m.id
			  )
from Managers m;