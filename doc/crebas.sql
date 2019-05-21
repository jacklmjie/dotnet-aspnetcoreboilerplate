/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2019/5/21 15:29:45                           */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('ClassRoom')
            and   type = 'U')
   drop table ClassRoom
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Student')
            and   type = 'U')
   drop table Student
go

/*==============================================================*/
/* Table: ClassRoom                                             */
/*==============================================================*/
create table ClassRoom (
   Id                   bigint               identity,
   Name                 nvarchar(200)        null,
   constraint PK_CLASSROOM primary key (Id)
)
go

/*==============================================================*/
/* Table: Student                                               */
/*==============================================================*/
create table Student (
   Id                   bigint               identity,
   Name                 nvarchar(200)        not null,
   Sex                  int                  not null,
   Age                  int                  not null,
   ClassRoomId          int                  not null,
   constraint PK_STUDENT primary key (Id)
)
go

