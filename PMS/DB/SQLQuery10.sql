select Name,
(select count(Id) from [Identity].tbl_1033_GeneralPublic as tb_10 where tb_10.district=dist.Code) CountGPC,
(select count(Id) from [Identity].tbl_1033_HealthManager as tbl_h where tbl_h.district=dist.Code) CountGPH
from tbl_Districts dist
order by Name

--select count(*) from [Identity].tbl_1033_GeneralPublic where district='037001'
--select count(*) from [Identity].tbl_1033_HealthManager where district='037001'