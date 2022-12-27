select gp.complainId,com.Id from [Identity].tbl_Complaints com
INNER JOIN [Identity].tbl_1033_GeneralPublic gp ON com.Id=gp.complainId
where com.srcId=1

select Id from [Identity].tbl_Complaints com where com.srcId=1 
and Id not in (select complainId from [Identity].tbl_1033_GeneralPublic)

delete from [Identity].tbl_Complaints where Id in (
select Id from [Identity].tbl_Complaints com where com.srcId=1 
and Id not in (select complainId from [Identity].tbl_1033_GeneralPublic)
)