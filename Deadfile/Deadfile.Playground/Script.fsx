// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "Library1.fs"
open Deadfile.Playground

// Define your library scripting code here
let s =
    @"Adur District Council
Allerdale Borough Council
Amber Valley Borough Council
Arun District Council
Ashfield District Council
Ashford Borough Council
Aylesbury Vale District Council
Babergh District Council
Barnet London Borough
Barnsley Metropolitan Borough Council
Barrow-in-Furness Borough Council
Basildon District Council
Basingstoke and Deane Borough Council
Bassetlaw District Council
Bath and North East Somerset Council
Bedford Borough Council
Birmingham City Council
Blaby District Council
Blackburn with Darwen Borough Council
Blackpool Borough Council
Blaenau Gwent County Borough Council
Bolsover District Council
Bolton Metropolitan Borough Council
Borough of Broxbourne Council
Borough of Poole
Boston Borough Council
Bournemouth Borough Council
Bracknell Forest Borough Council
Bradford Metropolitan District Council
Braintree District Council
Breckland District Council
Brecon Beacons National Park
Brent London Borough Council
Brentwood Borough Council
Bridgend County Borough Council
Brighton and Hove City Council
Bristol City Council
Broadland District Council
Broads Authority
Bromsgrove District Council
Broxtowe Borough Council
Buckinghamshire County Council
Burnley Borough Council
Bury Metropolitan Borough Council
Caerphilly County Borough Council
Calderdale Metropolitan Borough Council
Cambridge City Council
Cambridgeshire County Council
Camden Council
Cannock Chase District Council
Canterbury City Council
Cardiff Council
Carlisle City Council
Carmarthenshire County Council
Castle Point Borough Council
Central Bedfordshire Council
Ceredigion County  Council
Charnwood Borough Council
Chelmsford City Council
Cheltenham Borough Council
Cherwell District Council
Cheshire East Council
Cheshire West and Chester Council
Chesterfield Borough Council
Chichester District Council
Chiltern District Council
Chorley Borough Council
Christchurch Borough Council
City and County of Swansea
City of London
City of York Council
Colchester Borough Council
Conwy County Borough Council
Copeland Borough Council
Corby Borough Council
Cornwall Council
Cotswold District Council
Coventry City Council
Craven District Council
Crawley Borough Council
Croydon Council
Cumbria County Council
Cyngor Bwrdeistref Sirol Rhondda Cynon Taf
Cyngor Sir Dinas a Sir Caerdydd
Dacorum Borough Council
Darlington Borough Council
Dartford Borough Council
Dartmoor National Park Authority
Daventry District Council
Denbighshire County Council
Derby City Council
Derbyshire County Council
Derbyshire Dales District Council
Devon County Council
Doncaster Metropolitan Borough Council
Dorset County Council
Dover District Council
Dudley Metropolitan Borough Council
Durham County Council
Ealing Council London Borough
East Cambridgeshire District Council
East Devon District Council
East Dorset District Council
East Hampshire District Council
East Hertfordshire Council
East Lindsey District Council
East Northamptonshire District Council
East Riding of Yorkshire Council
East Staffordshire Borough Council
East Sussex County Council
Eastbourne Borough Council
Eastleigh Borough Council
Eden District Council
Elmbridge Borough Council
Enfield Council
Epping Forest District Council
Epsom and Ewell Borough Council
Erewash Borough Council
Essex County Council
Exeter City Council
Exmoor National Park
Fareham Borough Council
Fenland District Council
Flintshire County Council
Forest Heath District Council
Forest of Dean District Council
Fylde Borough Council
Gateshead Metropolitan Borough Council
Gedling Borough Council
Gloucester City Council
Gloucestershire County Council
Gosport Borough Council
Gravesham Borough Council
Great Yarmouth Borough Council
Guildford Borough Council
Gwynedd Council
Halton Borough Council
Hambleton District Council
Hampshire County Council
Harborough District Council
Haringey Council London Borough
Harlow Council
Harrogate Borough Council
Harrow Council London Borough
Hart District Council
Hartlepool Borough Council
Hastings Borough Council
Havant Borough Council
Herefordshire Council
Hertfordshire County Council
Hertsmere Borough Council
High Peak Borough Council
Hinckley and Bosworth Borough Council
Horsham District Council
Hounslow Council London Borough
Hull City Council
Huntingdonshire District Council
Hyndburn Borough Council
Ipswich Borough Council
Isle Of Wight Council
Isle of Anglesey County Council
Isles of Scilly Council
Islington Council
Kent County Council
Kettering Borough Council
Kings Lynn and West Norfolk Borough Council
Kingston upon Thames Royal Borough of
Kirklees Metropolitan Council
Knowsley Metropolitan Borough Council
Lake District National Park
Lambeth Council London Borough
Lancashire County Council
Lancaster City Council
Leeds City Council
Leicester City Council
Leicestershire County Council
Lewes District Council
Lichfield District Council
Lincoln City Council
Lincolnshire County Council
Liverpool City Council
London Borough Of Bexley
London Borough of Barking and Dagenham
The London Borough of Bromley
London Borough of Hackney
London Borough of Havering
London Borough of Hillingdon
London Borough of Lewisham
London Borough of Newham Council
London Borough of Redbridge
London Borough of Richmond upon Thames
London Borough of Tower Hamlets
London Legacy Development Corporation
Luton Borough Council
Maidstone Borough Council
Maldon District Council
Malvern Hills District Council
Manchester City Council
Mansfield District Council
Medway Council
Melton Borough Council
Mendip District Council
Merthyr Tydfil County Borough Council
Merton Council London Borough
Mid Suffolk District Council
Mid Sussex District Council
Middlesbrough Borough Council
Milton Keynes Council
Mole Valley District Council
Monmouthshire County Council
Neath Port Talbot County Borough Council
New Forest District Council
New Forest National Park
Newark and Sherwood District Council
Newcastle City Council
Newcastle under Lyme Borough Council
Newport City Council
Norfolk County Council
North Devon Council
North Dorset District Council
North East Derbyshire District Council
North East Lincolnshire Council
North Hertfordshire District Council
North Kesteven District Council
North Lincolnshire Council
North Somerset Council
North Tyneside Council
North Warwickshire Borough Council
North West Leicestershire District Council
North York Moors National Park
North Yorkshire County Council
Northampton Borough Council
Northamptonshire County Council
Northumberland County Council
Northumberland National Park Authority
Norwich City Council
Nottingham City Council
Nottinghamshire County Council
Nuneaton and Bedworth Borough Council
Oadby and Wigston Borough Council
Oldham Metropolitan Borough Council
Oxford City Council
Oxfordshire County Council
Pembrokeshire Coast National Park
Pembrokeshire County Council
Pendle Borough Council
Peterborough City Council
Plymouth City Council
Portsmouth City Council
Powys County Council
Preston City Council
Purbeck District Council
Reading Borough Council
Redcar and Cleveland Borough Council
Redditch Borough Council
Reigate and Banstead Borough Council
Rhondda Cynon Taf County Borough Council
Ribble Valley Borough Council
Rochdale Metropolitan Borough Council
Rochford District Council
Rossendale Borough Council
Rother District Council
Rotherham MBC
Royal Borough of Greenwich
Royal Borough of Windsor and Maidenhead
Rugby Borough Council
Runnymede Borough Council
Rushcliffe Borough Council
Rushmoor Borough Council
Rutland County Council
Ryedale District Council
Salford City Council
Sandwell Metropolitan Borough Council
Scarborough Borough Council
Sedgemoor District Council
Sefton Council
Sevenoaks District Council
Sheffield City Council
Shepway District Council
Shropshire Council
Slough Borough Council
Snowdonia National Park
Solihull Metropolitan Borough Council
Somerset County Council
South Bucks District Council
South Cambridgeshire District Council
South Derbyshire District Council
South Downs National Park Authority
South Downs National Park Authority (Adur DC)
South Downs National Park Authority (Arun DC)
South Downs National Park Authority (Brighton and Hove City Council)
South Downs National Park Authority (Chichester DC)
South Downs National Park Authority (East Hampshire DC)
South Downs National Park Authority (East Sussex County Council)
South Downs National Park Authority (Hampshire County Council)
South Downs National Park Authority (Horsham DC)
South Downs National Park Authority (Lewes DC)
South Downs National Park Authority (Mid Sussex DC)
South Downs National Park Authority (Wealden DC)
South Downs National Park Authority (Winchester City Council)
South Downs National Park Authority (Worthing BC)
South Gloucestershire Council
South Hams District Council
South Holland District Council
South Kesteven District Council
South Lakeland District Council
South Norfolk District Council
South Northamptonshire Council
South Oxfordshire District Council
South Ribble Borough Council
South Somerset District Council
South Staffordshire Council
Southampton City Council
Southend-on-Sea Borough Council
Southwark Council 
Spelthorne Borough Council
St Albans District Council
St Edmundsbury Borough Council
St Helens Council
Stafford Borough Council
Staffordshire County Council
Staffordshire Moorlands District Council
Stevenage Borough Council
Stockport Metropolitan Borough Council
Stockton-on-Tees Borough Council
Stoke-on-Trent City Council
Stratford on Avon District Council
Stroud District Council
Suffolk Coastal District Council
Suffolk County Council
Surrey County Council
Surrey Heath Borough Council
Sutton Council London Borough
Swale Borough Council
Swindon Borough Council
Tameside Metropolitan Borough Council
Tamworth Borough Council
Tandridge District Council
Taunton Deane Borough Council
Teignbridge District Council
Telford and Wrekin Council
Tendring District Council
Test Valley Borough Council
Tewkesbury Borough Council
Thanet District Council
The London Borough of Hammersmith and Fulham
The Royal Borough of Kensington and Chelsea
Three Rivers District Council
Thurrock Thames Gateway Development Corporation
Tonbridge and Malling Borough Council
Torbay Council
Torfaen County Borough Council
Torridge District Council
Trafford Metropolitan Borough Council
Tunbridge Wells Borough Council
Uttlesford District Council
Vale of Glamorgan Council
Vale of White Horse District Council
Wakefield Metropolitan District Council
Walsall Council
Waltham Forest Council
Wandsworth Borough Council
Warrington Borough Council
Warwick District Council
Warwickshire County Council
Watford Borough Council
Waveney District Council
Waverley Borough Council
Wealden District Council
Wellingborough Borough Council
Welwyn Hatfield Borough Council
West Berkshire Council
West Devon Borough Council
West Dorset District Council
West Lancashire Borough Council
West Lindsey District Council
West Oxfordshire District Council
West Somerset District Council
West Sussex County Council
Westminster City Council
Weymouth and Portland Borough Council
Wigan Council
Wiltshire Council
Winchester City Council
Wirral Metropolitan Borough Council
Woking Borough Council
Wokingham Borough Council
Wolverhampton City Council
Worcester City Council
Worcestershire County Council
Worthing Borough Council
Wrexham County Borough Council
Wychavon District Council
Wycombe District Council
Wyre Council
Wyre Forest District Council
Yorkshire Dales National Park
Blaenau Gwent County Borough Council
Brecon Beacons National Park
Bridgend County Borough Council
Caerphilly County Borough Council
Carmarthenshire County Council
Ceredigion County  Council
City and County of Swansea
Conwy County Borough Council
Cyngor Bwrdeistref Sirol Rhondda Cynon Taf
Cyngor Sir Dinas a Sir Caerdydd
Denbighshire County Council
Flintshire County Council
Gwynedd Council
Isle of Anglesey County Council
Merthyr Tydfil County Borough Council
Monmouthshire County Council
Neath Port Talbot County Borough Council
Newport City Council
Pembrokeshire Coast National Park
Pembrokeshire County Council
Rhondda Cynon Taf County Borough Council
Snowdonia National Park
Torfaen County Borough Council
Vale of Glamorgan Council
Wrexham County Borough Council"

s.Split([|'\n'|])
|> Set.ofSeq
|> Seq.iter
    (fun li ->
        printfn "                \"%s\"," li)
