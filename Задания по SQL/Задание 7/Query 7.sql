use Warehouse;
select m.First_Name as '���', m.Last_Name '�������', 
		(
			Select Count(w.Manager_Id) as '���������� �������'
			from Warehouse_Manager w
			Where m.id = w.Manager_Id
		 )	
from Managers m;