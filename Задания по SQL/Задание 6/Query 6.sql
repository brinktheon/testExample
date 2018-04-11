use Warehouse;
/*������� 6*/	
Select pt.Type as '��� ������',  Min(p.Price) as '����������� ����'
			from Products p
			inner join Products_Types pt
				on p.Type_Product_Id = pt.id
			Group by pt.Type
			Having sum(p.Price) > 1000
			