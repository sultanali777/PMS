--begin transaction  complainId
--tbl_Complaints
--tbl_1033_GeneralPublic
--tbl_1033_HealthManager
--tbl_ComplainHistory
--tbl_smsHistory
--tbl_Media
--tbl_Complainants
delete from [identity].tbl_Complainants where complainId in (
3,
4,
5,
8,
42,
43,
44,
47,
48,
347,
348,
349,
1347,
1348,
1349,
1350,
1351,
1353,
0,
1400
)
--rollback transaction