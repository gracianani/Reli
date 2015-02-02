create table HeatIndexAudit
(
	HeatIndexAutidId int primary key identity(1,1) not null,
	热力站ID int  not null,
	参考热指标 decimal(8,2) not null,
	UpdatedAt datetime not null,
	UpdatedByUserId int not null
)

create table TemperatureAudit
(
	TemperatureAuditId int primary key identity(1,1) not null,
	Temperature decimal(8,2) not null,
	UpdatedAt datetime not null,
	UpdatedByUserId int not null
)