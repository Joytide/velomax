import requests
from random import randint,randrange
import random
from datetime import datetime,timedelta
from copy import deepcopy



def populate_address(nb,outfile=None):
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    i=0
    while i<nb:
        data=requests.get("https://randomuser.me/api/").json()["results"][0]["location"]
        if data["street"]["name"].isascii() and data["city"].isascii():
            
            insert="INSERT INTO address VALUES ("+str(i)+",'"+data["street"]["name"].replace("'","")+"','"+data["country"].replace("'","")+"','"+str(data["postcode"])+"','"+data["city"].replace("'","")+"');"
            if outfile:
                with open(outfile,"a+") as file:
                    file.write(insert+"\n")
            print(insert)
            i+=1

    return [x for x in range(nb)] #ret primary keys


def populate_client(nbperson,nbcompagny,possible_addresses,outfile=None,):
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    i=0
    while i<nbperson:
        data=requests.get("https://randomuser.me/api/").json()["results"][0]
        surname=data["name"]["last"].replace("'","")
        clientname=data["name"]["first"].replace("'","")
        compagnyname="NULL"
        mail=data["email"].replace("'","")
        phone=data["phone"].replace("'","")
        contact="NULL"
        

        if surname.isascii() and clientname.isascii():
            addressid=possible_addresses.pop(randint(0,len(possible_addresses)-1))
            
            insert="INSERT INTO client VALUES ("+str(i)+",'"+clientname+"','"+surname+"',"+compagnyname+",'"+mail+"','"+str(phone)+"',"+contact+","+str(addressid)+");"
            if outfile:
                with open(outfile,"a+") as file:
                    file.write(insert+"\n")
            print(insert)
            i+=1

    compagnynames=['first_name', 'Vidoo', 'Thoughtbridge', 'Kare', 'Kare', 'Edgeclub', 'Trudoo', 'Zoonder', 'Skipfire', 'Thoughtstorm', 'Yombu', 'Flashdog', 'Innotype', 'Babblestorm', 'Skyvu', 'Jabbersphere', 'Livefish', 'Twitterwire', 'Dynazzy', 'JumpXS', 'Browsetype', 'Agimba', 'Eazzy', 'Zoomdog', 'Katz', 'Meemm', 'Gabcube', 'Blogpad', 'Kamba', 'Avaveo', 'Quire', 'Brainsphere', 'Demivee', 'Zoomdog', 'Devcast', 'Eayo', 'Oodoo', 'Skinix', 'Demimbu', 'JumpXS', 'Lazz', 'Vimbo', 'Trilith', 'Bubblebox', 'Realcube', 'Zoonoodle', 'Skilith', 'Avamba', 'Zooxo', 'Gabtune', 'Wordify', 'Jabbertype', 'Skimia', 'Feedfire', 'Oodoo', 'Photobug', 'Rhyloo', 'Skynoodle', 'Gabcube', 'Yotz', 'Yombu', 'Devcast', 'Flashdog', 'Yodel', 'Jetpulse', 'Youopia', 'Tavu', 'Chatterbridge', 'Zoozzy', 'Yombu', 'Zoomcast', 'Ntags', 'Fivebridge', 'Browsecat', 'Pixonyx', 'Yambee', 'Vipe', 'Cogibox', 'Brainlounge', 'Talane', 'Meejo', 'Shuffledrive', 'Realblab', 'Kanoodle', 'Zoovu', 'Wordpedia', 'Skidoo', 'Browsetype', 'Nlounge', 'Rhynyx', 'Dynava', 'Tagcat', 'Jetpulse', 'Skimia', 'Yozio', 'Wordtune', 'Cogidoo', 'Vinder', 'Skilith', 'Realblab', 'Skimia']
    if nbcompagny>len(compagnynames):
        print("ERROR YOU ASK FOR TOO MUCH COMPAGNY, ADD MORE https://www.mockaroo.com/")
    while i<nbcompagny+nbperson:
        data=requests.get("https://randomuser.me/api/").json()["results"][0]
        surname="NULL"
        clientname="NULL"
        compagnyname=compagnynames.pop(randint(0,len(compagnynames)-1)).replace("'","")
        mail=data["email"]
        phone=data["phone"]
        contact=data["name"]["first"].replace("'","")+" "+data["name"]["last"].replace("'","")
        

        if contact.isascii():
            addressid=possible_addresses.pop(randint(0,len(possible_addresses)-1))
            
            insert="INSERT INTO client VALUES ("+str(i)+","+clientname+","+surname+",'"+compagnyname+"','"+mail+"','"+str(phone)+"','"+contact+"',"+str(addressid)+");"
            if outfile:
                with open(outfile,"a+") as file:
                    file.write(insert+"\n")

            print(insert)
            i+=1

            
    return [x for x in range(nbperson+nbcompagny)],[x for x in range(nbperson)]



def random_date(start, end):
    delta = end - start
    int_delta = (delta.days * 24 * 60 * 60) + delta.seconds
    random_second = randrange(int_delta)
    return start + timedelta(seconds=random_second)



def populate_purchase(nb,clients,addresses,outfile=None):
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    i=0
    while i<nb:
        purchasenum=i
        deliverydate=random_date(datetime.strptime('1/1/2019', '%m/%d/%Y'),datetime.strptime('1/1/2020', '%m/%d/%Y'))
        orderdate=random_date(datetime.strptime('1/1/2020', '%m/%d/%Y'),datetime.strptime('1/1/2021', '%m/%d/%Y'))
        purchasecost=randint(100,1000)
        clientid=clients[randint(0,len(clients)-1)]
        addressid=addresses[randint(0,len(addresses)-1)]
        insert="INSERT INTO purchase VALUES ("+str(i)+",'"+deliverydate.strftime("%Y-%m-%d")+"','"+orderdate.strftime("%Y-%m-%d")+"',"+str(purchasecost)+","+str(clientid)+","+str(addressid)+");"
        if outfile:
            with open(outfile,"a+") as file:
                file.write(insert+"\n")
        print(insert)
        i+=1

    return [x for x in range(nb)]




def populate_part(outfile=None):
    parts_category={'Cadre': ['C32', 'C34', 'C76', 'C43', 'C44f', 'C43f', 'C01', 'C02', 'C15', 'C87', 'C87f', 'C25', 'C26'], 'Guidon': ['G7', 'G9', 'G12'], 'Freins': ['F3', 'F9'], 'Selle': ['S88', 'S35', 'S37', 'S02', 'S03', 'S36', 'S34', 'S87'], 'Derailleur Avant': ['DV133', 'DV17', 'DV87', 'DV57', 'DV15', 'DV41', 'DV132'], 'Derailleur Arriere': ['DR56', 'DR87', 'DR86', 'DR23', 'DR76', 'DR52'], 'Roue': ['R46', 'R47', 'R32', 'R18', 'R2','R45', 'R48', 'R12', 'R19', 'R1', 'R11', 'R44'], 'Reflecteurs': ['R02', 'R09', 'R10'], 'Pedalier': ['P12', 'P34', 'P1', 'P15'], 'Ordinateur': ['O2', 'O4'], 'Panier': ['S01', 'S05', 'S74', 'S73']}
    parts=[]

    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    
    for k,v in parts_category.items():
        for part in v:
            parts.append(part)
            insert="INSERT INTO part VALUES ('"+part+"','"+k+"','"+str(randint(0,15))+"','"+str(randint(10,50))+"');"
            if outfile:
                with open(outfile,"a+") as file:
                    file.write(insert+"\n")
            print(insert)

    return parts

def populate_supplier(addresses,outfile=None):
    suppliernames=['Quimba', 'Kazio', 'Devcast', 'Jaloo', 'Zoombox', 'LiveZ', 'Zoozzy', 'Zoombeat', 'Skalith', 'Zoozzy', 'Oodoo', 'DabZ', 'Kwinu', 'Eidel', 'Twiyo', 'Yodo', 'Jazzy', 'Agimba', 'Tagchat', 'Wordtune', 'Camido', 'Eimbee', 'Topicstorm', 'Jabberstorm', 'Jabberstorm', 'Skiptube', 'Rhynoodle', 'Riffwire', 'Mycat', 'Realpoint', 'Livetube', 'Twimbo', 'Skipfire', 'Trilith', 'Twimm', 'Twinte', 'Skinder', 'Browsebug', 'Flipbug', 'Skyndu', 'Skyndu', 'Zoombeat', 'Roodel', 'Feednation', 'Oyoloo', 'Rhynoodle', 'Zava', 'Fatz', 'Edgepulse', 'Twinder', 'Skimia', 'Yamia', 'Trunyx', 'Abatz', 'Eayo', 'Tambee', 'Twinte', 'Oyonder', 'Rooxo', 'Skipstorm', 'Voonyx', 'Realbridge', 'Eazzy', 'Tambee', 'Photobug', 'Centidel', 'Yotz', 'Vimbo', 'Dabvine', 'Viva', 'Edgepulse', 'Centizu', 'Wikizz', 'Meembee', 'Kaymbo', 'Youbridge', 'Mynte', 'Gevee', 'Meedoo', 'Skiba', 'Abata', 'Fiveclub', 'Dazzlesphere', 'Jaxspan', 'Katz', 'Mycat', 'Dabjam', 'Wikizz', 'Pixope', 'Jaxworks', 'Youtags', 'Skynoodle', 'Kazu', 'Cogidoo', 'Dablist', 'Oyonder', 'Jabberstorm', 'Demivee', 'Photobean', 'Voomm']
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    sirets=[]
    while len(addresses)>0:
        data=requests.get("https://randomuser.me/api/").json()["results"][0]
        contactname=data["name"]["first"].replace("'","")+" "+data["name"]["last"].replace("'","")
        if contactname.isascii():
            siret=str(randint(100000000000,99999999999999)).ljust(14,"0")
            sirets.append(siret)
            suppliername=suppliernames.pop(randint(0,len(suppliernames)-1))
            label="NULL"
            addressid=addresses.pop(randint(0,len(addresses)-1))
            insert="INSERT INTO supplier VALUES ('"+siret+"','"+suppliername+"','"+contactname+"',"+label+",'"+str(addressid)+"');"
            if outfile:
                with open(outfile,"a+") as file:
                    file.write(insert+"\n")
            print(insert)
    return sirets


def populate_bicycle(outfile=None):
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    bicycleids=[]
    bicycles={101: {'bname': 'Kilimandjaro', 'size': 'Adultes', 'cost': '569', 'type': 'VTT'}, 102: {'bname': 'NorthPole', 'size': 'Adultes', 'cost': '329', 'type': 'VTT'}, 103: {'bname': 'MontBlanc', 'size': 'Jeunes', 'cost': '399', 'type': 'VTT'}, 104: {'bname': 'Hooligan', 'size': 'Jeunes', 'cost': '199', 'type': 'VTT'}, 105: {'bname': 'Orleans', 'size': 'Hommes', 'cost': '229', 'type': 'Velo de course'}, 106: {'bname': 'Orleans', 'size': 'Dames', 'cost': '229', 'type': 'Velo de course'}, 107: {'bname': 'BlueJay', 'size': 'Hommes', 'cost': '349', 'type': 'Velo de course'}, 108: {'bname': 'BlueJay', 'size': 'Dames', 'cost': '349', 'type': 'Velo de course'}, 109: {'bname': 'Trail Explorer', 'size': 'Filles', 'cost': '129', 'type': 'Classique'}, 110: {'bname': 'Trail Explorer', 'size': 'Garcons', 'cost': '129', 'type': 'Classique'}, 111: {'bname': 'Night Hawk', 'size': 'Jeunes', 'cost': '189', 'type': 'Classique'}, 112: {'bname': 'Tierra Verde', 'size': 'Hommes', 'cost': '199', 'type': 'Classique'}, 113: {'bname': 'Tierra Verde', 'size': 'Dames', 'cost': '199', 'type': 'Classique'}, 114: {'bname': 'Mud Zinger I', 'size': 'Jeunes', 'cost': '279', 'type': 'BMX'}, 115: {'bname': 'Mud Zinger II', 'size': 'Adultes', 'cost': '359 ', 'type': 'BMX'}}
    for bicycleid,bicycledata in bicycles.items():
        bicycleids.append(bicycleid)
        bname=bicycledata["bname"]
        size=bicycledata["size"]
        btype=bicycledata["type"]
        cost=bicycledata["cost"]
        introddate=random_date(datetime.strptime('1/1/2012', '%m/%d/%Y'),datetime.strptime('1/1/2020', '%m/%d/%Y'))
        deprecdate=random_date(datetime.strptime('1/1/2022', '%m/%d/%Y'),datetime.strptime('1/1/2035', '%m/%d/%Y'))
        stockbicyclenumber=randint(0,5)

        insert="INSERT INTO bicycle VALUES ("+str(bicycleid)+",'"+bname+"','"+size+"',"+str(cost)+",'"+btype+"','"+introddate.strftime("%Y-%m-%d")+"','"+deprecdate.strftime("%Y-%m-%d")+"',"+str(stockbicyclenumber)+");"
        if outfile:
            with open(outfile,"a+") as file:
                file.write(insert+"\n")
        print(insert)
    return bicycleids


def populate_loyaltyprog(outfile=None):
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    programnums=[]
    programs={1: {'desc': 'Fidelio', 'fee': '15', 'length': 1, 'discount': 5}, 2: {'desc': 'Fidelio Or', 'fee': '25', 'length': 2, 'discount': 8}, 3: {'desc': 'Fidelio Platine', 'fee': '60', 'length': 2, 'discount': 10}, 4: {'desc': 'Fidelio Max', 'fee': '100', 'length': 3, 'discount': 12}}
    for programnum,progdata in programs.items():
        programnums.append(programnum)
        desc=progdata["desc"]
        fee=progdata["fee"]
        length=progdata["length"]
        discount=progdata["discount"]

        insert="INSERT INTO loyaltyprogram VALUES ("+str(programnum)+","+str(fee)+","+str(length)+","+str(discount)+",'"+desc+"');"
        if outfile:
            with open(outfile,"a+") as file:
                file.write(insert+"\n")
        print(insert)
    return programnums



def populate_providedpart(partnums,suppliers,outfile=None):

    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    provided={}
    while len(set([x for k,v in provided.items() for x in v ]))<len(partnums):
        for supplier in suppliers:
            for i in range(randint(5,20)): # 5 to 20 parts per supplier
                partnum=partnums[randint(0,len(partnums)-1)]
                if supplier in provided:
                    if partnum in provided[supplier]:
                        continue
                    else:
                        provided[supplier].append(partnum)
                else:
                    provided[supplier]=[partnum]

                price=randint(10,100)
                introddate=random_date(datetime.strptime('1/1/2012', '%m/%d/%Y'),datetime.strptime('1/1/2020', '%m/%d/%Y'))
                deprecdate=random_date(datetime.strptime('1/1/2022', '%m/%d/%Y'),datetime.strptime('1/1/2035', '%m/%d/%Y'))
                delay=randint(1,10)

                insert="INSERT INTO providedpart VALUES ('"+partnum+"','"+supplier+"',"+str(price)+",'"+introddate.strftime("%Y-%m-%d")+"','"+deprecdate.strftime("%Y-%m-%d")+"',"+str(delay)+");"
                if outfile:
                    with open(outfile,"a+") as file:
                        file.write(insert+"\n")
                print(insert)
    return provided



def populate_ordered(purchasenums, partnums, bicycleids, outfile=None):
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")

    
    for purchasenum in purchasenums:
        if randint(0,2)==0: #Only parts order
            possible_parts=deepcopy(partnums)
            number_parts=randint(2,10)
            for i in range(number_parts):
                partnum=possible_parts.pop(randint(0,len(possible_parts)-1))
                weights = [1] * 70 + [2] * 20 + [3] * 10
                orderedpartnb = random.choice(weights)
                insert="INSERT INTO orderedpart VALUES ("+str(purchasenum)+",'"+partnum+"',"+str(orderedpartnb)+");"
                if outfile:
                    with open(outfile,"a+") as file:
                        file.write(insert+"\n")
                print(insert)
        elif randint(0,1): # Only bicycle order
            possible_bicycles=deepcopy(bicycleids)
            number_bicycles=random.choice([1,1,1,1,1,2,2,2,3])
            for i in range(number_bicycles):
                bicycleid=possible_bicycles.pop(randint(0,len(possible_bicycles)-1))
                weights = [1] * 90 + [2] * 5 + [3] * 5
                orderedbicyclenb = random.choice(weights)
                insert="INSERT INTO orderedbicycle VALUES ("+str(purchasenum)+","+str(bicycleid)+","+str(orderedbicyclenb)+");"
                if outfile:
                    with open(outfile,"a+") as file:
                        file.write(insert+"\n")
                print(insert)

        else: #Bicycle+parts order
            possible_parts=deepcopy(partnums)
            number_parts=randint(2,10)
            for i in range(number_parts):
                partnum=possible_parts.pop(randint(0,len(possible_parts)-1))
                weights = [1] * 70 + [2] * 20 + [3] * 10
                orderedpartnb = random.choice(weights)
                insert="INSERT INTO orderedpart VALUES ("+str(purchasenum)+",'"+partnum+"',"+str(orderedpartnb)+");"
                if outfile:
                    with open(outfile,"a+") as file:
                        file.write(insert+"\n")
            print(insert)
            possible_bicycles=deepcopy(bicycleids)
            number_bicycles=random.choice([1,1,1,1,1,2,2,2,3])
            for i in range(number_bicycles):
                bicycleid=possible_bicycles.pop(randint(0,len(possible_bicycles)-1))
                weights = [1] * 90 + [2] * 5 + [3] * 5
                orderedbicyclenb = random.choice(weights)
                insert="INSERT INTO orderedbicycle VALUES ("+str(purchasenum)+","+str(bicycleid)+","+str(orderedbicyclenb)+");"
                if outfile:
                    with open(outfile,"a+") as file:
                        file.write(insert+"\n")
                print(insert)



def populate_subscription(clientids,programnums,outfile=None):
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    for client in clientids:
        if randint(0,1):
            programnum=randint(1,4)
            programstartdate=random_date(datetime.strptime('6/1/2020', '%m/%d/%Y'),datetime.strptime('1/1/2021', '%m/%d/%Y'))
            insert="INSERT INTO subscription VALUES ("+str(client)+","+str(programnum)+",'"+programstartdate.strftime("%Y-%m-%d")+"');"
            if outfile:
                with open(outfile,"a+") as file:
                    file.write(insert+"\n")
            print(insert)


def populate_composition():
    if outfile:
        with open(outfile,"a+") as file:
            file.write("\n")
    compositions={101: ['C32', 'G7', 'F3', 'S88', 'DV133', 'DR56', 'R45', 'R46', 'P12', 'O2'], 102: ['C34', 'G7', 'F3', 'S88', 'DV17', 'DR87', 'R48', 'R47', 'P12'], 103: ['C76', 'G7', 'F3', 'S88', 'DV17', 'DR87', 'R48', 'R47', 'P12', 'O2'], 104: ['C76', 'G7', 'F3', 'S88', 'DV87', 'DR86', 'R12', 'R32', 'P12'], 105: ['C43', 'G9', 'F9', 'S37', 'DV57', 'DR86', 'R19', 'R18', 'R02', 'P34'], 106: ['C44f', 'G9', 'F9', 'S35', 'DV57', 'DR86', 'R19', 'R18', 'R02', 'P34'], 107: ['C43', 'G9', 'F9', 'S37', 'DV57', 'DR87', 'R19', 'R18', 'R02', 'P34', 'O4'], 108: ['C43f', 'G9', 'F9', 'S35', 'DV57', 'DR87', 'R19', 'R18', 'R02', 'P34', 'O4'], 109: ['C01', 'G12', 'S02', 'R1', 'R2', 'R09', 'P1', 'S01'], 110: ['C02', 'G12', 'S03', 'R1', 'R2', 'R09', 'P1', 'S05'], 111: ['C15', 'G12', 'F9', 'S36', 'DV15', 'DR23', 'R11', 'R12', 'R10', 'P15', 'S74'], 112: ['C87', 'G12', 'F9', 'S36', 'DV41', 'DR76', 'R11', 'R12', 'R10', 'P15', 'S74'], 113: ['C87f', 'G12', 'F9', 'S34', 'DV41', 'DR76', 'R11', 'R12', 'R10', 'P15', 'S73'], 114: ['C25', 'G7', 'F3', 'S87', 'DV132', 'DR52', 'R44', 'R47', 'P12'], 115: ['C26', 'G7', 'F3', 'S87', 'DV133', 'DR52', 'R44', 'R47', 'P12']}
    for bicycleid,composition in compositions.items():
        for part in composition:
            insert="INSERT INTO composition VALUES ('"+part+"',"+str(bicycleid)+");"
            if outfile:
                with open(outfile,"a+") as file:
                    file.write(insert+"\n")
            print(insert)


outfile="script.sql"


"""
import os
os.remove("nonce.sql")

"""



populate_purchase

addr = populate_address(20,outfile)
clients,clientsindiv = populate_client(5,5,deepcopy(addr[:10]),outfile)
purchasenums = populate_purchase(50,clients,addr[:10],outfile)

partnums = populate_part(outfile)
'''
suppliers = populate_supplier(deepcopy(addr[10:]),outfile)
bicycleids = populate_bicycle(outfile)
programnums = populate_loyaltyprog(outfile)
populate_providedpart(partnums,suppliers,outfile)
populate_ordered(deepcopy(purchasenums), deepcopy(partnums), deepcopy(bicycleids), outfile)
populate_subscription(clientsindiv,programnums,outfile)
populate_composition()
'''