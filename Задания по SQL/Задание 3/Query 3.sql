use Warehouse;
/*������� 3*/
select p.Name as '������������ ��������', pt.Type as '��� ������', ps.Name as '�������������' 
from Products p
INNER join Products_Types pt
	on p.Type_Product_Id = pt.id
INNER join Producers ps
	on p.Producer_Id = ps.id
where p.Price > 50;
