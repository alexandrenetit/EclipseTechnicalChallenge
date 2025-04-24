î
RC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.API\Program.cs
var 
builder 
= 
WebApplication 
. 
CreateBuilder *
(* +
args+ /
)/ 0
;0 1
builder 
. 
Services 
. 
AddControllers 
(  
)  !
;! "
builder 
. 
Services 
. -
!AddApplicationDependencyInjection 2
(2 3
)3 4
;4 5
builder 
. 
Services 
. (
AddDomainDependencyInjection -
(- .
). /
;/ 0
builder 
. 
Services 
. 0
$AddInfrastructureDependencyInjection 5
(5 6
builder6 =
.= >
Configuration> K
)K L
;L M
builder 
. 
Services 
. -
!AddFluentValidationAutoValidation 2
(2 3
)3 4
;4 5
builder 
. 
Services 
. $
ConfigureHttpJsonOptions )
() *
options* 1
=>2 4
{5 6
options 
. 
SerializerOptions 
. 

Converters (
.( )
Add) ,
(, -
new- 0#
JsonStringEnumConverter1 H
(H I
)I J
)J K
;K L
} 
) 
; 
builder 
. 
Services 
. #
AddEndpointsApiExplorer (
(( )
)) *
;* +
builder 
. 
Services 
. 
AddSwaggerGen 
( 
)  
;  !
builder 
. 
Services 
. 
AddCors 
( 
options  
=>! #
{$ %
options   
.   
AddDefaultPolicy   
(   
builder   $
=>  % '
{  ( )
builder!! 
.!! 
WithOrigins!! 
(!! 
$str!! 3
)!!3 4
."" 	
AllowAnyMethod""	 
("" 
)"" 
.## 	
AllowAnyHeader##	 
(## 
)## 
;## 
}$$ 
)$$ 
;$$ 
}%% 
)%% 
;%% 
var(( 
app(( 
=(( 	
builder((
 
.(( 
Build(( 
((( 
)(( 
;(( 
app** 
.** *
UseExceptionHandlingMiddleware** "
(**" #
)**# $
;**$ %
app++ 
.++ 

UseRouting++ 
(++ 
)++ 
;++ 
app.. 
... 
UseCors.. 
(.. 
).. 
;.. 
app11 
.11 

UseSwagger11 
(11 
)11 
;11 
app22 
.22 
UseSwaggerUI22 
(22 
)22 
;22 
app44 
.44 
UseHttpsRedirection44 
(44 
)44 
;44 
app66 
.66 
UseAuthentication66 
(66 
)66 
;66 
app77 
.77 
UseAuthorization77 
(77 
)77 
;77 
app99 
.99 
MapControllers99 
(99 
)99 
;99 
app;; 
.;; 
Run;; 
(;; 
);; 	
;;;	 
æ
qC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.API\Middleware\ExceptionHandlingMiddleware.cs
	namespace 	
TaskManagement
 
. 
API 
. 

Middleware '
;' (
public 
class '
ExceptionHandlingMiddleware (
{ 
private 
readonly 
RequestDelegate $
_next% *
;* +
private 
readonly 
ILogger 
< '
ExceptionHandlingMiddleware 8
>8 9
_logger: A
;A B
public 
'
ExceptionHandlingMiddleware &
(& '
RequestDelegate' 6
next7 ;
,; <
ILogger= D
<D E'
ExceptionHandlingMiddlewareE `
>` a
loggerb h
)h i
{		 
_next

 
=

 
next

 
;

 
_logger 
= 
logger 
; 
} 
public 

async 
Task 
Invoke 
( 
HttpContext (
httpContext) 4
)4 5
{ 
try 
{ 	
await 
_next 
( 
httpContext #
)# $
;$ %
} 	
catch 
( 
	Exception 
ex 
) 
{ 	
if 
( 
ex 
. 
InnerException !
!=" $
null% )
)) *
{ 
_logger 
. 
LogError  
(  !
$str! E
,E F
exG I
.I J
InnerExceptionJ X
.X Y
GetTypeY `
(` a
)a b
.b c
ToStringc k
(k l
)l m
,m n
exo q
.q r
InnerException	r Ä
.
Ä Å
Message
Å à
)
à â
;
â ä
} 
else 
{ 
_logger 
. 
LogError  
(  !
$str! E
,E F
exG I
.I J
GetTypeJ Q
(Q R
)R S
.S T
ToStringT \
(\ ]
)] ^
,^ _
ex` b
.b c
Messagec j
)j k
;k l
} 
httpContext 
. 
Response  
.  !

StatusCode! +
=, -
$num. 1
;1 2
await   
httpContext   
.   
Response   &
.  & '
WriteAsJsonAsync  ' 7
(  7 8
new  8 ;
{  < =
Message  > E
=  F G
ex  H J
.  J K
Message  K R
,  R S
Type  T X
=  Y Z
ex  [ ]
.  ] ^
GetType  ^ e
(  e f
)  f g
.  g h
ToString  h p
(  p q
)  q r
}  s t
)  t u
;  u v
}!! 	
}"" 
}## 
public&& 
static&& 
class&& 1
%ExceptionHandlingMiddlewareExtensions&& 9
{'' 
public(( 

static(( 
IApplicationBuilder(( %*
UseExceptionHandlingMiddleware((& D
(((D E
this((E I
IApplicationBuilder((J ]
builder((^ e
)((e f
{)) 
return** 
builder** 
.** 
UseMiddleware** $
<**$ %'
ExceptionHandlingMiddleware**% @
>**@ A
(**A B
)**B C
;**C D
}++ 
},, ó(
jC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.API\Controllers\WorkItemsController.cs
	namespace 	
TaskManagement
 
. 
API 
. 
Controllers (
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public		 

class		 
WorkItemsController		 $
:		% &
ControllerBase		' 5
{

 
private 
readonly 
IWorkItemService )
_workItemService* :
;: ;
public 
WorkItemsController "
(" #
IWorkItemService# 3
workItemService4 C
)C D
{ 	
_workItemService 
= 
workItemService .
;. /
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
ActionResult &
<& '#
WorkItemDetailsResponse' >
>> ?
>? @
GetByIdA H
(H I
GuidI M
idN P
)P Q
{ 	
var 
workItem 
= 
await  
_workItemService! 1
.1 2#
GetWorkItemDetailsAsync2 I
(I J
idJ L
)L M
;M N
return 
Ok 
( 
workItem 
) 
;  
} 	
[ 	
HttpPost	 
] 
public 
async 
Task 
< 
ActionResult &
<& '
WorkItemResponse' 7
>7 8
>8 9
Create: @
(@ A!
CreateWorkItemRequestA V
requestW ^
)^ _
{ 	
var 
workItem 
= 
await  
_workItemService! 1
.1 2
CreateWorkItemAsync2 E
(E F
requestF M
)M N
;N O
return 
CreatedAtAction "
(" #
nameof# )
() *
GetById* 1
)1 2
,2 3
new4 7
{8 9
id: <
== >
workItem? G
.G H
IdH J
}K L
,L M
workItemN V
)V W
;W X
} 	
[   	
HttpPut  	 
(   
$str   
)   
]   
public!! 
async!! 
Task!! 
<!! 
ActionResult!! &
<!!& '
WorkItemResponse!!' 7
>!!7 8
>!!8 9
Update!!: @
(!!@ A
Guid!!A E
id!!F H
,!!H I!
UpdateWorkItemRequest!!J _
request!!` g
,!!g h
[!!i j

FromHeader!!j t
]!!t u
Guid!!v z

modifiedBy	!!{ Ö
)
!!Ö Ü
{"" 	
var## 
workItem## 
=## 
await##  
_workItemService##! 1
.##1 2
UpdateWorkItemAsync##2 E
(##E F
id##F H
,##H I
request##J Q
,##Q R

modifiedBy##S ]
)##] ^
;##^ _
return$$ 
Ok$$ 
($$ 
workItem$$ 
)$$ 
;$$  
}%% 	
['' 	

HttpDelete''	 
('' 
$str'' 
)'' 
]'' 
public(( 
async(( 
Task(( 
<(( 
IActionResult(( '
>((' (
Delete(() /
(((/ 0
Guid((0 4
id((5 7
)((7 8
{)) 	
await** 
_workItemService** "
.**" #
DeleteWorkItemAsync**# 6
(**6 7
id**7 9
)**9 :
;**: ;
return++ 
	NoContent++ 
(++ 
)++ 
;++ 
},, 	
[.. 	
HttpPost..	 
(.. 
$str.. )
)..) *
]..* +
public// 
async// 
Task// 
<// 
ActionResult// &
<//& '
CommentResponse//' 6
>//6 7
>//7 8

AddComment//9 C
(//C D
Guid//D H

workItemId//I S
,//S T
AddCommentRequest//U f
request//g n
)//n o
{00 	
var11 
comment11 
=11 
await11 
_workItemService11  0
.110 1
AddCommentAsync111 @
(11@ A

workItemId11A K
,11K L
request11M T
)11T U
;11U V
return22 
CreatedAtAction22 "
(22" #
nameof22# )
(22) *
GetById22* 1
)221 2
,222 3
new224 7
{228 9
id22: <
=22= >

workItemId22? I
}22J K
,22K L
comment22M T
)22T U
;22U V
}33 	
}44 
}55 Ú
hC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.API\Controllers\ReportsController.cs
	namespace 	
TaskManagement
 
. 
API 
. 
Controllers (
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public		 

class		 
ReportsController		 "
:		# $
ControllerBase		% 3
{

 
private 
readonly 
IReportService '
_reportService( 6
;6 7
public 
ReportsController  
(  !
IReportService! /
reportService0 =
)= >
{ 	
_reportService 
= 
reportService *
;* +
} 	
[ 	
HttpGet	 
( 
$str 
) 
]  
public 
async 
Task 
< 
ActionResult &
<& '%
PerformanceReportResponse' @
>@ A
>A B 
GetPerformanceReportC W
(W X
[ 
	FromQuery 
] 
Guid 
userId #
,# $
[ 
	FromQuery 
] 
DateTime  
?  !
fromDate" *
=+ ,
null- 1
,1 2
[ 
	FromQuery 
] 
DateTime  
?  !
toDate" (
=) *
null+ /
)/ 0
{ 	
var 
defaultFromDate 
=  !
DateTime" *
.* +
UtcNow+ 1
.1 2
AddDays2 9
(9 :
-: ;
$num; =
)= >
;> ?
var 
defaultToDate 
= 
DateTime  (
.( )
UtcNow) /
;/ 0
var 
report 
= 
await 
_reportService -
.- .*
GeneratePerformanceReportAsync. L
(L M
userId 
, 
fromDate 
?? 
defaultFromDate +
,+ ,
toDate 
?? 
defaultToDate '
)' (
;( )
return   
Ok   
(   
report   
)   
;   
}!! 	
}"" 
}## π
iC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.API\Controllers\ProjectsController.cs
	namespace 	
TaskManagement
 
. 
API 
. 
Controllers (
{ 
[ 
ApiController 
] 
[ 
Route 

(
 
$str 
) 
] 
public		 

class		 
ProjectsController		 #
:		$ %
ControllerBase		& 4
{

 
private 
readonly 
IProjectService (
_projectService) 8
;8 9
public 
ProjectsController !
(! "
IProjectService" 1
projectService2 @
)@ A
{ 	
_projectService 
= 
projectService ,
;, -
} 	
[ 	
HttpGet	 
( 
$str !
)! "
]" #
public 
async 
Task 
< 
ActionResult &
<& '
IEnumerable' 2
<2 3
ProjectResponse3 B
>B C
>C D
>D E

GetByOwnerF P
(P Q
GuidQ U
ownerIdV ]
)] ^
{ 	
var 
projects 
= 
await  
_projectService! 0
.0 1#
GetProjectsByOwnerAsync1 H
(H I
ownerIdI P
)P Q
;Q R
return 
Ok 
( 
projects 
) 
;  
} 	
[ 	
HttpGet	 
( 
$str 
) 
] 
public 
async 
Task 
< 
ActionResult &
<& '"
ProjectDetailsResponse' =
>= >
>> ?
GetById@ G
(G H
GuidH L
idM O
)O P
{ 	
var 
project 
= 
await 
_projectService  /
./ 0"
GetProjectDetailsAsync0 F
(F G
idG I
)I J
;J K
return 
Ok 
( 
project 
) 
; 
} 	
[   	
HttpPost  	 
]   
public!! 
async!! 
Task!! 
<!! 
ActionResult!! &
<!!& '
ProjectResponse!!' 6
>!!6 7
>!!7 8
Create!!9 ?
(!!? @ 
CreateProjectRequest!!@ T
request!!U \
)!!\ ]
{"" 	
var## 
project## 
=## 
await## 
_projectService##  /
.##/ 0
CreateProjectAsync##0 B
(##B C
request##C J
)##J K
;##K L
return$$ 
CreatedAtAction$$ "
($$" #
nameof$$# )
($$) *
GetById$$* 1
)$$1 2
,$$2 3
new$$4 7
{$$8 9
id$$: <
=$$= >
project$$? F
.$$F G
Id$$G I
}$$J K
,$$K L
project$$M T
)$$T U
;$$U V
}%% 	
['' 	

HttpDelete''	 
('' 
$str'' 
)'' 
]'' 
public(( 
async(( 
Task(( 
<(( 
IActionResult(( '
>((' (
Delete(() /
(((/ 0
Guid((0 4
id((5 7
)((7 8
{)) 	
await** 
_projectService** !
.**! "
DeleteProjectAsync**" 4
(**4 5
id**5 7
)**7 8
;**8 9
return++ 
	NoContent++ 
(++ 
)++ 
;++ 
},, 	
}-- 
}.. 