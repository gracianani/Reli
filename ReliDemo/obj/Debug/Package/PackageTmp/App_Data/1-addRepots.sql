
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Plan_analyse]') AND type in (N'U'))
DROP TABLE [dbo].Rpt_Plan_analyse
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create table Rpt_Plan_analyse
(
   日期 date,
   室外温度 decimal(4,1),

   销售1   decimal(6,2), --执行到位率
   创合1   decimal(6,2),
   特力昆1 decimal(6,2),
   天禹1   decimal(6,2),
   合计1   decimal(6,2),
   
   销售2   decimal(10,2), --超标站总面积 (万平米)
   创合2   decimal(10,2),
   特力昆2 decimal(10,2),
   天禹2   decimal(10,2),
   合计2   decimal(10,2),

   销售3   decimal(10,2), --有效站总面积 (万平米)
   创合3   decimal(10,2),
   特力昆3 decimal(10,2),
   天禹3   decimal(10,2),
   合计3   decimal(10,2),
   
   销售4   smallint, --有效站数
   创合4   smallint,
   特力昆4 smallint,
   天禹4   smallint,
   合计4   smallint,

   reserved1 nvarchar(100),
   reserved2 decimal(6),
   reserved3 nvarchar(200),
   reserved4 int
)

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_Rpt_Plan_analyse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Proc_Rpt_Plan_analyse]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[Proc_Rpt_Plan_analyse](
  @start_date as date,
  @end_date as date
)
AS
truncate table Rpt_Plan_analyse
insert Rpt_Plan_analyse(日期,室外温度) select 日期,朝阳平均 from Weather_4 where 日期 between @start_date and @end_date
create table #ttt_1(ItemId int,day1 date,a1 decimal(14,1),a2 decimal(14,1),d1 SMALLINT)
create table #ttt_2(ItemId int,day1 date,a3 decimal(14,1))

insert #ttt_1(ItemId,day1,a1,d1) select c.公司ID,b.日期,sum(b.投入面积),count(*) from StationAccuHistory b,Stations c 
  where (c.ItemId=b.热力站ID) and (b.日期 BETWEEN @start_date and @end_date) and (b.报警 is null) and (b.投入面积>100)
       group by c.公司ID,b.日期 order by 2,1

insert #ttt_2(ItemId,day1,a3) select c.公司ID,b.日期,sum(b.投入面积) from StationAccuHistory b,Stations c 
  where (c.ItemId=b.热力站ID) and (b.日期 BETWEEN @start_date and @end_date) and (b.报警 is null) and (b.投入面积>100)
       and (b.采暖GJ>b.核算GJ) group by c.公司ID,b.日期

update #ttt_1 set a2=b.a3 from #ttt_2 b where #ttt_1.ItemId=b.ItemId and #ttt_1.day1=b.day1
--insert Rpt_Plan_analyse(日期,室外温度) select 日期,朝阳平均 from Weather_4 where 日期 between @start_date and @end_date
update Rpt_Plan_analyse set 创合2=b.a2*0.0001,创合3=b.a1*0.0001,创合4=b.d1 from #ttt_1 b,Rpt_Plan_analyse a 
  where a.日期=b.day1 and b.ItemId=(select ItemId from Companies where 公司 like '创合%')
update Rpt_Plan_analyse set 销售2=b.a2*0.0001,销售3=b.a1*0.0001,销售4=b.d1 from #ttt_1 b,Rpt_Plan_analyse a 
  where a.日期=b.day1 and b.ItemId=(select ItemId from Companies where 公司 like '销售%')
update Rpt_Plan_analyse set 特力昆2=b.a2*0.0001,特力昆3=b.a1*0.0001,特力昆4=b.d1 from #ttt_1 b,Rpt_Plan_analyse a 
  where a.日期=b.day1 and b.ItemId=(select ItemId from Companies where 公司 like '特力昆%')
update Rpt_Plan_analyse set 天禹2=b.a2*0.0001,天禹3=b.a1*0.0001,天禹4=b.d1 from #ttt_1 b,Rpt_Plan_analyse a 
  where a.日期=b.day1 and b.ItemId=(select ItemId from Companies where 公司 like '天禹%')
update Rpt_Plan_analyse set 合计1=销售1+创合1+特力昆1+天禹1,合计2=销售2+创合2+特力昆2+天禹2,合计3=销售3+创合3+特力昆3+天禹3,合计4=销售4+创合4+特力昆4+天禹4
update Rpt_Plan_analyse set 销售1=(销售3-销售2)*100.0/销售3,创合1=(创合3-创合2)*100.0/创合3,
   特力昆1=(特力昆3-特力昆2)*100.0/特力昆3,天禹1=(天禹3-天禹2)*100.0/天禹3,合计1=(合计3-合计2)*100.0/合计3
insert Rpt_Plan_analyse(销售2,创合2,特力昆2,天禹2,合计2,销售3,创合3,特力昆3,天禹3,合计3) 
  select sum(销售2),sum(创合2),sum(特力昆2),sum(天禹2),sum(合计2),sum(销售3),sum(创合3),sum(特力昆3),sum(天禹3),sum(合计3)
     from Rpt_Plan_analyse
update Rpt_Plan_analyse set 销售1=(销售3-销售2)*100.0/销售3,创合1=(创合3-创合2)*100.0/创合3,
   特力昆1=(特力昆3-特力昆2)*100.0/特力昆3,天禹1=(天禹3-天禹2)*100.0/天禹3,合计1=(合计3-合计2)*100.0/合计3
   where 日期 is null
drop table #ttt_1
drop table #ttt_2

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Rate_analyse]') AND type in (N'U'))
DROP TABLE [dbo].Rpt_Rate_analyse
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create table Rpt_Rate_analyse(
  ItemId int,
  公司 nvarchar(100),
  有效日供热总面积  decimal(18,2),
  超标日总供热面积  decimal(18,2),
  未超标日总供热面积 decimal(18,2),
  采暖季执行到位率 decimal(6,2)
)

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_Rpt_Rate_analyse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Proc_Rpt_Rate_analyse]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE  [dbo].[Proc_Rpt_Rate_analyse](
  @start_date as date,
  @end_date as date
)
AS
truncate table Rpt_Rate_analyse
insert into Rpt_Rate_analyse(ItemId,公司)  select ItemId,公司 from Companies where 公司 not like '科利源%'

create table #ttt(ItemId int,a1 decimal(12,2))

insert #ttt(ItemId,a1) select a.ItemId,sum(b.投入面积) from Rpt_Rate_analyse a,StationAccuHistory b,Stations c 
  where (c.ItemId=b.热力站ID) and (b.日期 BETWEEN @start_date and @end_date) and (b.报警 is null) and (b.投入面积>100)
       and (a.ItemId=c.公司ID)  group by a.ItemId
update Rpt_Rate_analyse set 有效日供热总面积=b.a1 from #ttt b where Rpt_Rate_analyse.ItemId=b.ItemId
truncate table #ttt

insert #ttt(ItemId,a1) select a.ItemId,sum(b.投入面积) from Rpt_Rate_analyse a,StationAccuHistory b,Stations c 
  where (c.ItemId=b.热力站ID) and (b.日期 BETWEEN @start_date and @end_date) and (b.报警 is null) and (b.投入面积>100)
       and (a.ItemId=c.公司ID) and (b.采暖GJ>b.核算GJ) group by a.ItemId

update Rpt_Rate_analyse set 超标日总供热面积=b.a1 from #ttt b where Rpt_Rate_analyse.ItemId=b.ItemId
insert Rpt_Rate_analyse(公司,有效日供热总面积,超标日总供热面积) select '合计',sum(有效日供热总面积),sum(超标日总供热面积) from Rpt_Rate_analyse
update Rpt_Rate_analyse set 未超标日总供热面积=有效日供热总面积-超标日总供热面积
update Rpt_Rate_analyse set 采暖季执行到位率=未超标日总供热面积/有效日供热总面积*100.0
drop table #ttt

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Data_analyse]') AND type in (N'U'))
DROP TABLE [dbo].Rpt_Data_analyse
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create table Rpt_Data_analyse
(
   ItemId int,
   热力站名称 nvarchar(250),
   管理单位   nvarchar(150),
   投入面积     decimal(10,2),
   超核算次数  smallint,
   比例  decimal(6,2),
   d1   smallint,
   d2   smallint,
   d3   smallint,
   d4   smallint,
   reserved1 nvarchar(100),
   reserved2 decimal(6),
   reserved3 nvarchar(200),
)
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_Rpt_Rate_analyse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Proc_Rpt_Rate_analyse]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[Proc_Rpt_Data_analyse](
  @ItemId as int,
  @start_date as date,
  @end_date as date
)
AS
truncate table Rpt_Data_analyse
insert into Rpt_Data_analyse(ItemId,热力站名称,管理单位,投入面积)  select ItemId,热力站名称,管理单位,站面积 from Stations
    where 公司ID=@ItemId  and 来源ID<2 order by 热力站名称
create table #ttt(ItemId int,d1 smallint)
insert #ttt(ItemId,d1) select a.ItemId,count(*) c1 from Rpt_Data_analyse a,StationAccuHistory b 
  where (a.ItemId=b.热力站ID) and (b.日期 BETWEEN  @start_date and @end_date) and (b.报警 is null)
  group by a.ItemId
update Rpt_Data_analyse set d1=b.d1 from #ttt b where Rpt_Data_analyse.ItemId=b.ItemId
truncate table #ttt

insert #ttt(ItemId,d1) select a.ItemId,count(*) c1 from Rpt_Data_analyse a,StationAccuHistory b 
  where (a.ItemId=b.热力站ID) and (b.日期 BETWEEN  @start_date and @end_date) and (b.报警 is null)
       and (b.采暖GJ>b.核算GJ)  group by a.ItemId
update Rpt_Data_analyse set 超核算次数=b.d1 from #ttt b where Rpt_Data_analyse.ItemId=b.ItemId
update Rpt_Data_analyse set 比例=超核算次数*100.0/d1

drop table #ttt
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Rpt_Station_analyse]') AND type in (N'U'))
DROP TABLE [dbo].Rpt_Station_analyse
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create table Rpt_Station_analyse
(
   ItemId int,
   热力站名称 nvarchar(250),
   分公司     nvarchar(100),
   管理单位   nvarchar(150),
   站面积     decimal(10,2),
   参考热指标 decimal(8,2) NULL ,
   数据来源 nvarchar(50),
   是否重点站 bit NULL ,
   收费性质 nvarchar(100),
   生产热源   nvarchar(100),
   统计天数   smallint,
   有效天数   smallint,
   无效天数   smallint,
   超标天数   smallint,
   未超标天数 smallint,
   站天数到位率  decimal(6,2),
   站有效数据率  decimal(6,2),
   站无效数据率  decimal(6,2),
   站天数超标率  decimal(6,2),
   有效日计划供热量  decimal(10,1),
   有效日核算供热量  decimal(10,1),
   有效日实际供热量  decimal(10,1),
   有效日供热总面积  decimal(10,2),
   超标日总供热面积  decimal(10,2),
   未超标日总供热面积  decimal(10,2),
   reserved1 nvarchar(100),
   reserved2 decimal(6),
   reserved3 nvarchar(200),
   reserved4 int
)

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proc_Rpt_Station_analyse]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[Proc_Rpt_Station_analyse]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  Proc_Rpt_Station_analyse(
  @ItemId as int,
  @start_date as date,
  @end_date as date
)
AS
truncate table Rpt_Station_analyse

insert into Rpt_Station_analyse(ItemId,热力站名称,分公司,管理单位,站面积,参考热指标,数据来源,是否重点站,收费性质,生产热源)
   select ItemId,热力站名称,公司,管理单位,站面积,参考热指标,数据来源,是否重点站,收费性质,生产热源 from Stations
    where 公司ID=@ItemId and 来源ID<2 order by 热力站名称

create table #ttt(ItemId int,d1 smallint,g1 decimal(10,1),g2 decimal(10,1),g3 decimal(10,1),a1 decimal(10,2),a2 decimal(10,2))

insert #ttt(ItemId,d1) select a.ItemId,count(*) c1 from Rpt_Station_analyse a,StationAccuHistory b 
  where (a.ItemId=b.热力站ID) and (b.日期 BETWEEN @start_date and @end_date) 
  group by a.ItemId
update Rpt_Station_analyse set 统计天数=b.d1 from #ttt b where Rpt_Station_analyse.ItemId=b.ItemId
truncate table #ttt

insert #ttt(ItemId,d1) select a.ItemId,count(*) c1 from Rpt_Station_analyse a,StationAccuHistory b 
  where (a.ItemId=b.热力站ID) and (b.日期 BETWEEN @start_date and @end_date) and (b.报警 is null)
  group by a.ItemId
update Rpt_Station_analyse set 有效天数=b.d1 from #ttt b where Rpt_Station_analyse.ItemId=b.ItemId
update Rpt_Station_analyse set 无效天数=统计天数-有效天数 where 统计天数>有效天数
truncate table #ttt

insert #ttt(ItemId,d1) select a.ItemId,count(*) c1 from Rpt_Station_analyse a,StationAccuHistory b 
  where (a.ItemId=b.热力站ID) and (b.日期 BETWEEN  @start_date and @end_date) and (b.报警 is null)
  and (b.采暖GJ>b.核算GJ)
  group by a.ItemId
update Rpt_Station_analyse set 超标天数=b.d1 from #ttt b where Rpt_Station_analyse.ItemId=b.ItemId
truncate table #ttt
update Rpt_Station_analyse set 站天数到位率=未超标天数*100.0/统计天数
update Rpt_Station_analyse set 未超标天数=统计天数,站天数到位率=100.0 where 未超标天数 is null
update Rpt_Station_analyse set 站有效数据率=有效天数*100.0/统计天数
update Rpt_Station_analyse set 站无效数据率=无效天数*100.0/统计天数
update Rpt_Station_analyse set 站天数超标率=超标天数*100.0/统计天数

insert #ttt(ItemId,g1,g2,g3,a1) select a.ItemId,sum(计划GJ),sum(核算GJ),sum(采暖GJ),sum(投入面积) from Rpt_Station_analyse a,StationAccuHistory b 
  where (a.ItemId=b.热力站ID) and (b.日期 BETWEEN @start_date and @end_date) and (b.报警 is null) and (投入面积>100)
  group by a.ItemId
update Rpt_Station_analyse set 有效日计划供热量=b.g1,有效日核算供热量=b.g2,有效日实际供热量=b.g3,有效日供热总面积=b.a1 from #ttt b where Rpt_Station_analyse.ItemId=b.ItemId
truncate table #ttt

insert #ttt(ItemId,a1) select a.ItemId,sum(投入面积) from Rpt_Station_analyse a,StationAccuHistory b 
  where (a.ItemId=b.热力站ID) and (b.日期 BETWEEN  @start_date and @end_date) and (b.报警 is null)
  and (b.采暖GJ>b.核算GJ)
  group by a.ItemId
update Rpt_Station_analyse set 超标日总供热面积=b.a1,未超标日总供热面积=有效日供热总面积-b.a1 from #ttt b where Rpt_Station_analyse.ItemId=b.ItemId
drop table #ttt
GO