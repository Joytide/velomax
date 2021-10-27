drop database if exists velomax;
create database velomax;
use velomax;

CREATE TABLE purchase (purchasenum int NOT NULL, deliverydate datetime, orderdate datetime, clientid int, addressid int);

CREATE TABLE part (partnum int NOT NULL, partdesc varchar(40), stockpartnumber int);

CREATE TABLE supplier (siret int NOT NULL, suppliername varchar(40), contactname varchar(40), label varchar(40), addressid int);

CREATE TABLE bicycle (bicycleid int NOT NULL, bname varchar(40), size varchar(40), cost decimal, type varchar(40), introddate datetime, deprecdate datetime, stockbicyclenumber int);

CREATE TABLE `client` (clientid int NOT NULL, clientname varchar(40), surname varchar(40), companyname varchar(40), mail varchar(40), phone varchar(40), totalexpense int, contact varchar(40), addressid  int);

CREATE TABLE loyaltyprogram (programnum varchar(40) NOT NULL, fee int, length int, discount int, programdesc varchar(40));

CREATE TABLE address (addressid int NOT NULL, street varchar(40), region varchar(40), postalcode varchar(40), city varchar(40));

CREATE TABLE providedpart (partnum int NOT NULL, siret int NOT NULL, price decimal, introdate datetime, deprecateddate datetime, delay int);

CREATE TABLE orderedpart (purchasenum int NOT NULL, partnum int NOT NULL, orderedpartnb int);

CREATE TABLE composition (partnum int NOT NULL, bicycleid int NOT NULL);

CREATE TABLE subscription (clientid int NOT NULL, programnum varchar(40) NOT NULL, programstartdate datetime);

CREATE TABLE orderedbicycle (purchasenum int NOT NULL, bicycleid int NOT NULL, orderedbicyclenb int);

ALTER TABLE purchase ADD CONSTRAINT PK_purchase PRIMARY KEY (purchasenum);

ALTER TABLE part ADD CONSTRAINT PK_part PRIMARY KEY (partnum);

ALTER TABLE supplier ADD CONSTRAINT PK_supplier PRIMARY KEY (siret);

ALTER TABLE bicycle ADD CONSTRAINT PK_bicycle PRIMARY KEY (bicycleid);

ALTER TABLE `client` ADD CONSTRAINT PK_client PRIMARY KEY (clientid);

ALTER TABLE loyaltyprogram ADD CONSTRAINT PK_loyaltyprogram PRIMARY KEY (programnum);

ALTER TABLE address ADD CONSTRAINT PK_address PRIMARY KEY (addressid);

ALTER TABLE providedpart ADD CONSTRAINT PK_providedpart PRIMARY KEY (partnum, siret);

ALTER TABLE orderedpart ADD CONSTRAINT PK_orderedpart PRIMARY KEY (purchasenum, partnum);

ALTER TABLE composition ADD CONSTRAINT PK_composition PRIMARY KEY (partnum, bicycleid);

ALTER TABLE subscription ADD CONSTRAINT PK_subscription PRIMARY KEY (clientid, programnum);

ALTER TABLE orderedbicycle ADD CONSTRAINT PK_orderedbicycle PRIMARY KEY (purchasenum, bicycleid);

ALTER TABLE purchase ADD CONSTRAINT FK_purchase_clientid FOREIGN KEY (clientid) REFERENCES client (clientid);

ALTER TABLE purchase ADD CONSTRAINT FK_purchase_addressid FOREIGN KEY (addressid) REFERENCES address (addressid);

ALTER TABLE supplier ADD CONSTRAINT FK_supplier_addressid FOREIGN KEY (addressid) REFERENCES address (addressid);

ALTER TABLE `client` ADD CONSTRAINT FK_client_addressid FOREIGN KEY (addressid) REFERENCES address (addressid);

ALTER TABLE providedpart ADD CONSTRAINT FK_providedpart_partnum FOREIGN KEY (partnum) REFERENCES part (partnum);

ALTER TABLE providedpart ADD CONSTRAINT FK_providedpart_siret FOREIGN KEY (siret) REFERENCES supplier (siret);

ALTER TABLE orderedpart ADD CONSTRAINT FK_orderedpart_purchasenum FOREIGN KEY (purchasenum) REFERENCES purchase (purchasenum);

ALTER TABLE orderedpart ADD CONSTRAINT FK_orderedpart_partnum FOREIGN KEY (partnum) REFERENCES part (partnum);

ALTER TABLE composition ADD CONSTRAINT FK_composition_partnum FOREIGN KEY (partnum) REFERENCES part (partnum);

ALTER TABLE composition ADD CONSTRAINT FK_composition_bicycleid FOREIGN KEY (bicycleid) REFERENCES bicycle (bicycleid);

ALTER TABLE subscription ADD CONSTRAINT FK_subscription_clientid FOREIGN KEY (clientid) REFERENCES client (clientid);

ALTER TABLE subscription ADD CONSTRAINT FK_subscription_programnum FOREIGN KEY (programnum) REFERENCES loyaltyprogram (programnum);

ALTER TABLE orderedbicycle ADD CONSTRAINT FK_orderedbicycle_purchasenum FOREIGN KEY (purchasenum) REFERENCES purchase (purchasenum);

ALTER TABLE orderedbicycle ADD CONSTRAINT FK_orderedbicycle_bicycleid FOREIGN KEY (bicycleid) REFERENCES bicycle (bicycleid);

