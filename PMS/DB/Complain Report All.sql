select 
                                 ComplainID = com.Id,
                                 Category = cat.Name,
                                 Soruce = coS.Description,
                                 Department = dep.Name,
                                 status = sta.Description,
                                 description = coH.Description,
                                 dateCreated = FORMAT (coH.date_Created, 'dd-MM-yyyy')
from  [Identity].tbl_Complaints com
                             join   [Identity].tbl_Departments dep on com.deptId = dep.Id
                             join   [Identity].tbl_Categories cat on com.categId = cat.Id
                             join   [Identity].tbl_ComplainSources coS on com.srcId = coS.Id
                             join   [Identity].tbl_ComplainHistory coH on com.Id = coH.complainId
                             join   [Identity].tbl_Status sta on coH.statusId = sta.Id
							  where coH.description not like '%Testing%'
							 order by com.id desc
							
                           
                           