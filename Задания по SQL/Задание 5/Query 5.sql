use Warehouse;
/*������� 5*/
Select pt.Type as '��� ��������', Min(p.Price) '����������� ����'
			from Products p
			inner join Products_Types pt
				on p.Type_Product_Id = pt.id
			Group by pt.Type