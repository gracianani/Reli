create table heatSourceSequences
(
	heatSourceSequenceId int primary key identity(1,1),
	heatSourceId int foreign key references heatsources,
	sequence int
)