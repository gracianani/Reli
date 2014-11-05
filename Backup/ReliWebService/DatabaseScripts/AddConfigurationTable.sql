create table configuration
(
	configurationId int primary key identity(1,1),
	比计划多耗超标线 decimal(10,2) not null,
	比核算多耗超标线 decimal(10,2) not null,
	回温超标线 decimal(10,2) not null,
	流量超标线 decimal(10,2) not null
)

insert into configuration(比计划多耗超标线,比核算多耗超标线,回温超标线,流量超标线)
values(20,20,45,40)