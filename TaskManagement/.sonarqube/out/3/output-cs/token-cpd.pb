°
|C:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Validators\UpdateWorkItemRequestValidator.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %

Validators% /
;/ 0
public 
class *
UpdateWorkItemRequestValidator +
:, -
AbstractValidator. ?
<? @!
UpdateWorkItemRequest@ U
>U V
{ 
public 
*
UpdateWorkItemRequestValidator )
() *
)* +
{		 
RuleFor

 
(

 
x

 
=>

 
x

 
.

 
Title

 
)

 
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, R
)R S
. 
When 
( 
x 
=> 
x 
. 
Title 
!= !
null" &
)& '
;' (
RuleFor 
( 
x 
=> 
x 
. 
Description "
)" #
. 
MaximumLength 
( 
$num 
)  
.  !
WithMessage! ,
(, -
$str- Z
)Z [
. 
When 
( 
x 
=> 
x 
. 
Description $
!=% '
null( ,
), -
;- .
RuleFor 
( 
x 
=> 
x 
. 
DueDate 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ :
): ;
. 
When 
( 
x 
=> 
x 
. 
DueDate  
.  !
HasValue! )
)) *
;* +
RuleFor 
( 
x 
=> 
x 
. 
Status 
) 
. 
IsInEnum 
( 
) 
. 
WithMessage #
(# $
$str$ :
): ;
. 
When 
( 
x 
=> 
x 
. 
Status 
.  
HasValue  (
)( )
;) *
} 
} Î
|C:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Validators\CreateWorkItemRequestValidator.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %

Validators% /
;/ 0
public 
class *
CreateWorkItemRequestValidator +
:, -
AbstractValidator. ?
<? @!
CreateWorkItemRequest@ U
>U V
{ 
public 
*
CreateWorkItemRequestValidator )
() *
)* +
{		 
RuleFor

 
(

 
x

 
=>

 
x

 
.

 
Title

 
)

 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ 7
)7 8
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, R
)R S
;S T
RuleFor 
( 
x 
=> 
x 
. 
Description "
)" #
. 
MaximumLength 
( 
$num 
)  
.  !
WithMessage! ,
(, -
$str- Z
)Z [
;[ \
RuleFor 
( 
x 
=> 
x 
. 
DueDate 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ :
): ;
;; <
RuleFor 
( 
x 
=> 
x 
. 
Priority 
)  
. 
IsInEnum 
( 
) 
. 
WithMessage #
(# $
$str$ <
)< =
;= >
RuleFor 
( 
x 
=> 
x 
. 
	ProjectId  
)  !
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ <
)< =
. 
NotEqual 
( 
Guid 
. 
Empty  
)  !
.! "
WithMessage" -
(- .
$str. O
)O P
;P Q
RuleFor 
( 
x 
=> 
x 
. 
	CreatedBy  
)  !
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ <
)< =
. 
NotEqual 
( 
Guid 
. 
Empty  
)  !
.! "
WithMessage" -
(- .
$str. O
)O P
;P Q
} 
}   Ö
{C:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Validators\CreateProjectRequestValidator.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %

Validators% /
;/ 0
public 
class )
CreateProjectRequestValidator *
:+ ,
AbstractValidator- >
<> ? 
CreateProjectRequest? S
>S T
{ 
public 
)
CreateProjectRequestValidator (
(( )
)) *
{		 
RuleFor

 
(

 
x

 
=>

 
x

 
.

 
Name

 
)

 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ >
)> ?
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, Y
)Y Z
;Z [
RuleFor 
( 
x 
=> 
x 
. 
Description "
)" #
. 
MaximumLength 
( 
$num 
) 
.  
WithMessage  +
(+ ,
$str, X
)X Y
;Y Z
RuleFor 
( 
x 
=> 
x 
. 
OwnerId 
) 
. 
NotEmpty 
( 
) 
. 
WithMessage #
(# $
$str$ :
): ;
. 
NotEqual 
( 
Guid 
. 
Empty  
)  !
.! "
WithMessage" -
(- .
$str. M
)M N
;N O
} 
} æ_
kC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Services\WorkItemService.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %
Services% -
;- .
public 
class 
WorkItemService 
: 
IWorkItemService /
{ 
private 
readonly 
IWorkItemRepository (
_workItemRepository) <
;< =
private 
readonly 
IProjectRepository '
_projectRepository( :
;: ;
private 
readonly 
IUnitOfWork  
_unitOfWork! ,
;, -
private 
readonly "
IWorkItemServiceDomain +"
_domainWorkItemService, B
;B C
public 

WorkItemService 
( 
IWorkItemRepository 
workItemRepository .
,. /
IProjectRepository 
projectRepository ,
,, -
IUnitOfWork 

unitOfWork 
, "
IWorkItemServiceDomain !
domainWorkItemService 4
)4 5
{ 
_workItemRepository 
= 
workItemRepository 0
;0 1
_projectRepository 
= 
projectRepository .
;. /
_unitOfWork 
= 

unitOfWork  
;  !"
_domainWorkItemService 
=  !
domainWorkItemService! 6
;6 7
} 
public   

async   
Task   
<   
WorkItemResponse   &
>  & '
CreateWorkItemAsync  ( ;
(  ; <!
CreateWorkItemRequest  < Q
request  R Y
)  Y Z
{!! 
var"" 
	validator"" 
="" 
new"" *
CreateWorkItemRequestValidator"" :
("": ;
)""; <
;""< =
var## 
validationResult## 
=## 
await## $
	validator##% .
.##. /
ValidateAsync##/ <
(##< =
request##= D
)##D E
;##E F
if%% 

(%% 
!%% 
validationResult%% 
.%% 
IsValid%% %
)%%% &
throw&& 
new&& 
ValidationException&& )
(&&) *
validationResult&&* :
.&&: ;
Errors&&; A
)&&A B
;&&B C
var(( 
project(( 
=(( 
await(( 
_projectRepository(( .
.((. /
GetByIdAsync((/ ;
(((; <
request((< C
.((C D
	ProjectId((D M
)((M N
;((N O
if)) 

()) 
project)) 
==)) 
null)) 
))) 
{** 	
throw++ 
new++  
KeyNotFoundException++ *
(++* +
$str+++ >
)++> ?
;++? @
},, 	"
_domainWorkItemService.. 
... $
ValidateWorkItemCreation.. 7
(..7 8
project..8 ?
,..? @
request..A H
...H I
Priority..I Q
)..Q R
;..R S
var00 
workItem00 
=00 
new00 
WorkItem00 #
(00# $
Guid11 
.11 
NewGuid11 
(11 
)11 
,11 
request22 
.22 
Title22 
,22 
request33 
.33 
Description33 
,33  
request44 
.44 
DueDate44 
,44 
request55 
.55 
Priority55 
,55 
request66 
.66 
	ProjectId66 
,66 
request77 
.77 
	CreatedBy77 
)77 
;77 
project99 
.99 
AddWorkItem99 
(99 
workItem99 $
)99$ %
;99% &
await;; 
_workItemRepository;; !
.;;! "
AddAsync;;" *
(;;* +
workItem;;+ 3
);;3 4
;;;4 5
await== 
_unitOfWork== 
.== 
CommitAsync== %
(==% &
)==& '
;==' (
return?? 
workItem?? 
.?? 

ToResponse?? "
(??" #
)??# $
;??$ %
}@@ 
publicBB 

asyncBB 
TaskBB 
<BB #
WorkItemDetailsResponseBB -
>BB- .#
GetWorkItemDetailsAsyncBB/ F
(BBF G
GuidBBG K

workItemIdBBL V
)BBV W
{CC 
varDD 
workItemDD 
=DD 
awaitDD 
_workItemRepositoryDD 0
.DD0 1
GetWithDetailsAsyncDD1 D
(DDD E

workItemIdDDE O
)DDO P
;DDP Q
ifEE 

(EE 
workItemEE 
==EE 
nullEE 
)EE 
{FF 	
throwGG 
newGG  
KeyNotFoundExceptionGG *
(GG* +
$strGG+ @
)GG@ A
;GGA B
}HH 	
returnJJ 
workItemJJ 
.JJ 
ToDetailsResponseJJ )
(JJ) *
)JJ* +
;JJ+ ,
}KK 
publicMM 

asyncMM 
TaskMM 
<MM 
WorkItemResponseMM &
>MM& '
UpdateWorkItemAsyncMM( ;
(MM; <
GuidMM< @

workItemIdMMA K
,MMK L!
UpdateWorkItemRequestMMM b
requestMMc j
,MMj k
GuidMMl p

modifiedByMMq {
)MM{ |
{NN 
varOO 
	validatorOO 
=OO 
newOO *
UpdateWorkItemRequestValidatorOO :
(OO: ;
)OO; <
;OO< =
varPP 
validationResultPP 
=PP 
awaitPP $
	validatorPP% .
.PP. /
ValidateAsyncPP/ <
(PP< =
requestPP= D
)PPD E
;PPE F
ifRR 

(RR 
!RR 
validationResultRR 
.RR 
IsValidRR %
)RR% &
throwSS 
newSS 
ValidationExceptionSS )
(SS) *
validationResultSS* :
.SS: ;
ErrorsSS; A
)SSA B
;SSB C
varUU 
workItemUU 
=UU 
awaitUU 
_workItemRepositoryUU 0
.UU0 1
GetByIdAsyncUU1 =
(UU= >

workItemIdUU> H
)UUH I
;UUI J
ifVV 

(VV 
workItemVV 
==VV 
nullVV 
)VV 
{WW 	
throwXX 
newXX  
KeyNotFoundExceptionXX *
(XX* +
$strXX+ @
)XX@ A
;XXA B
}YY 	
if[[ 

([[ 
request[[ 
.[[ 
Status[[ 
.[[ 
HasValue[[ #
)[[# $
{\\ 	
workItem]] 
.]] 
UpdateStatus]] !
(]]! "
request]]" )
.]]) *
Status]]* 0
.]]0 1
Value]]1 6
,]]6 7

modifiedBy]]8 B
)]]B C
;]]C D
}^^ 	
if`` 

(`` 
request`` 
.`` 
Title`` 
!=`` 
null`` !
||``" $
request``% ,
.``, -
Description``- 8
!=``9 ;
null``< @
||``A C
request``D K
.``K L
DueDate``L S
.``S T
HasValue``T \
)``\ ]
{aa 	
workItembb 
.bb 
UpdateDetailsbb "
(bb" #
requestcc 
.cc 
Titlecc 
??cc  
workItemcc! )
.cc) *
Titlecc* /
,cc/ 0
requestdd 
.dd 
Descriptiondd #
??dd$ &
workItemdd' /
.dd/ 0
Descriptiondd0 ;
,dd; <
requestee 
.ee 
DueDateee 
??ee  "
workItemee# +
.ee+ ,
DueDateee, 3
,ee3 4

modifiedByff 
)ff 
;ff 
workItemhh 
.hh 
MarkAsUpdatedhh "
(hh" #
)hh# $
;hh$ %
}ii 	
awaitkk 
_unitOfWorkkk 
.kk 
CommitAsynckk %
(kk% &
)kk& '
;kk' (
returnmm 
workItemmm 
.mm 

ToResponsemm "
(mm" #
)mm# $
;mm$ %
}nn 
publicpp 

asyncpp 
Taskpp 
DeleteWorkItemAsyncpp )
(pp) *
Guidpp* .

workItemIdpp/ 9
)pp9 :
{qq 
varrr 
workItemrr 
=rr 
awaitrr 
_workItemRepositoryrr 0
.rr0 1
GetByIdAsyncrr1 =
(rr= >

workItemIdrr> H
)rrH I
;rrI J
ifss 

(ss 
workItemss 
==ss 
nullss 
)ss 
{tt 	
throwuu 
newuu  
KeyNotFoundExceptionuu *
(uu* +
$struu+ @
)uu@ A
;uuA B
}vv 	
ifxx 

(xx 
workItemxx 
.xx 
Statusxx 
!=xx 
WorkItemStatusxx -
.xx- .
	Completedxx. 7
)xx7 8
{yy 	
throwzz 
newzz %
InvalidOperationExceptionzz /
(zz/ 0
$strzz0 a
)zza b
;zzb c
}{{ 	
await}} 
_workItemRepository}} !
.}}! "
DeleteAsync}}" -
(}}- .
workItem}}. 6
)}}6 7
;}}7 8
await~~ 
_unitOfWork~~ 
.~~ 
CommitAsync~~ %
(~~% &
)~~& '
;~~' (
} 
public
ÅÅ 

async
ÅÅ 
Task
ÅÅ 
<
ÅÅ 
CommentResponse
ÅÅ %
>
ÅÅ% &
AddCommentAsync
ÅÅ' 6
(
ÅÅ6 7
Guid
ÅÅ7 ;

workItemId
ÅÅ< F
,
ÅÅF G
AddCommentRequest
ÅÅH Y
request
ÅÅZ a
)
ÅÅa b
{
ÇÇ 
var
ÉÉ 
workItem
ÉÉ 
=
ÉÉ 
await
ÉÉ !
_workItemRepository
ÉÉ 0
.
ÉÉ0 1!
GetWithDetailsAsync
ÉÉ1 D
(
ÉÉD E

workItemId
ÉÉE O
)
ÉÉO P
;
ÉÉP Q
if
ÑÑ 

(
ÑÑ 
workItem
ÑÑ 
==
ÑÑ 
null
ÑÑ 
)
ÑÑ 
{
ÖÖ 	
throw
ÜÜ 
new
ÜÜ "
KeyNotFoundException
ÜÜ *
(
ÜÜ* +
$str
ÜÜ+ @
)
ÜÜ@ A
;
ÜÜA B
}
áá 	
workItem
ââ 
.
ââ 

AddComment
ââ 
(
ââ 
request
ââ #
.
ââ# $
Content
ââ$ +
,
ââ+ ,
request
ââ- 4
.
ââ4 5
AuthorId
ââ5 =
)
ââ= >
;
ââ> ?
var
ãã 

newComment
ãã 
=
ãã 
workItem
ãã !
.
ãã! "
Comments
ãã" *
.
ãã* +
Last
ãã+ /
(
ãã/ 0
)
ãã0 1
;
ãã1 2
var
çç 

newHistory
çç 
=
çç 
workItem
çç !
.
çç! "
History
çç" )
.
çç) *
Last
çç* .
(
çç. /
)
çç/ 0
;
çç0 1
await
èè !
_workItemRepository
èè !
.
èè! "
AddCommentAsync
èè" 1
(
èè1 2

newComment
èè2 <
)
èè< =
;
èè= >
await
êê !
_workItemRepository
êê !
.
êê! "
AddHistoryAsync
êê" 1
(
êê1 2

newHistory
êê2 <
)
êê< =
;
êê= >
await
íí 
_unitOfWork
íí 
.
íí 
CommitAsync
íí %
(
íí% &
)
íí& '
;
íí' (
return
îî 

newComment
îî 
.
îî 

ToResponse
îî $
(
îî$ %
)
îî% &
;
îî& '
}
ïï 
}ññ è#
iC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Services\ReportService.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %
Services% -
;- .
public 
class 
ReportService 
: 
IReportService +
{ 
private		 
readonly		 
IWorkItemRepository		 (
_workItemRepository		) <
;		< =
private

 
readonly

 
IUserRepository

 $
_userRepository

% 4
;

4 5
public 

ReportService 
( 
IWorkItemRepository ,
workItemRepository- ?
,? @
IUserRepositoryA P
userRepositoryQ _
)_ `
{ 
_workItemRepository 
= 
workItemRepository 0
;0 1
_userRepository 
= 
userRepository (
;( )
} 
public 

async 
Task 
< %
PerformanceReportResponse /
>/ 0*
GeneratePerformanceReportAsync1 O
(O P
GuidP T
userIdU [
,[ \
DateTime] e
fromDatef n
,n o
DateTimep x
toDatey 
)	 Ä
{ 
if 

( 
userId 
== 
Guid 
. 
Empty  
)  !
throw 
new !
ArgumentNullException +
(+ ,
$str, C
)C D
;D E
var 
requestingUser 
= 
await "
_userRepository# 2
.2 3
GetByIdAsync3 ?
(? @
userId@ F
)F G
;G H
if 

( 
requestingUser 
== 
null "
||# %
!& '
requestingUser' 5
.5 6
	IsManager6 ?
)? @
{ 	
throw 
new '
UnauthorizedAccessException 1
(1 2
$str2 `
)` a
;a b
} 	
var 
completedWorkItems 
=  
(! "
await" '
_workItemRepository( ;
.; <
GetAllAsync< G
(G H
)H I
)I J
.   
Where   
(   
wi   
=>   
wi   
.   
Status   "
==  # %
WorkItemStatus  & 4
.  4 5
	Completed  5 >
&&  ? A
wi!! 
.!! 
DueDate!! #
>=!!$ &
fromDate!!' /
&&!!0 2
wi"" 
."" 
DueDate"" #
<=""$ &
toDate""' -
)""- .
.## 
ToList## 
(## 
)## 
;## 
var&& 
allUsers&& 
=&& 
await&& 
_userRepository&& ,
.&&, -
GetAllAsync&&- 8
(&&8 9
)&&9 :
;&&: ;
var'' 
activeUsers'' 
='' 
allUsers'' "
.(( 
Where(( 
((( 
u(( 
=>(( 
!(( 
u(( 
.(( 
	IsManager(( $
&&((% '
completedWorkItems)) %
.))% &
Any))& )
())) *
wi))* ,
=>))- /
wi))0 2
.))2 3
Id))3 5
==))6 8
u))9 :
.)): ;
Id)); =
)))= >
)))> ?
.** 
ToList** 
(** 
)** 
;** 
var-- 
averageCompleted-- 
=-- 
activeUsers-- *
.--* +
Count--+ 0
>--1 2
$num--3 4
?.. 
(.. 
double.. 
).. 
completedWorkItems.. (
...( )
Count..) .
/../ 0
activeUsers..1 <
...< =
Count..= B
:// 
$num// 
;// 
return11 
new11 %
PerformanceReportResponse11 ,
(11, -
completedWorkItems22 
.22 
Count22 $
,22$ %
averageCompleted33 
,33 
fromDate44 
,44 
toDate55 
)55 
;55 
}66 
}77 µ.
jC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Services\ProjectService.cs
	namespace

 	
TaskManagement


 
.

 
Application

 $
.

$ %
Services

% -
;

- .
public 
class 
ProjectService 
: 
IProjectService -
{ 
private 
readonly 
IProjectRepository '
_projectRepository( :
;: ;
private 
readonly 
IUnitOfWork  
_unitOfWork! ,
;, -
public 

ProjectService 
( 
IProjectRepository ,
projectRepository- >
,> ?
IUnitOfWork@ K

unitOfWorkL V
)V W
{ 
_projectRepository 
= 
projectRepository .
;. /
_unitOfWork 
= 

unitOfWork  
;  !
} 
public 

async 
Task 
< 
ProjectResponse %
>% &
CreateProjectAsync' 9
(9 : 
CreateProjectRequest: N
requestO V
)V W
{ 
var 
	validator 
= 
new )
CreateProjectRequestValidator 9
(9 :
): ;
;; <
var 
validationResult 
= 
await $
	validator% .
.. /
ValidateAsync/ <
(< =
request= D
)D E
;E F
if 

( 
! 
validationResult 
. 
IsValid %
)% &
throw   
new   
ValidationException   )
(  ) *
validationResult  * :
.  : ;
Errors  ; A
)  A B
;  B C
var"" 
project"" 
="" 
new"" 
Project"" !
(""! "
Guid""" &
.""& '
NewGuid""' .
("". /
)""/ 0
,""0 1
request""2 9
.""9 :
Name"": >
,""> ?
request""@ G
.""G H
Description""H S
,""S T
request""U \
.""\ ]
OwnerId""] d
)""d e
;""e f
await$$ 
_projectRepository$$  
.$$  !
AddAsync$$! )
($$) *
project$$* 1
)$$1 2
;$$2 3
await&& 
_unitOfWork&& 
.&& 
CommitAsync&& %
(&&% &
)&&& '
;&&' (
return(( 
project(( 
.(( 

ToResponse(( !
(((! "
)((" #
;((# $
})) 
public++ 

async++ 
Task++ 
<++ 
IEnumerable++ !
<++! "
ProjectResponse++" 1
>++1 2
>++2 3#
GetProjectsByOwnerAsync++4 K
(++K L
Guid++L P
ownerId++Q X
)++X Y
{,, 
var-- 
projects-- 
=-- 
await-- 
_projectRepository-- /
.--/ 0
GetByOwnerIdAsync--0 A
(--A B
ownerId--B I
)--I J
;--J K
return.. 
projects.. 
... 
Select.. 
(.. 
p..  
=>..! #
p..$ %
...% &

ToResponse..& 0
(..0 1
)..1 2
)..2 3
;..3 4
}// 
public11 

async11 
Task11 
<11 "
ProjectDetailsResponse11 ,
>11, -"
GetProjectDetailsAsync11. D
(11D E
Guid11E I
	projectId11J S
)11S T
{22 
var33 
project33 
=33 
await33 
_projectRepository33 .
.33. /!
GetWithWorkItemsAsync33/ D
(33D E
	projectId33E N
)33N O
;33O P
if44 

(44 
project44 
==44 
null44 
)44 
{55 	
throw66 
new66  
KeyNotFoundException66 *
(66* +
$str66+ >
)66> ?
;66? @
}77 	
return99 
project99 
.99 
ToDetailsResponse99 (
(99( )
)99) *
;99* +
}:: 
public<< 

async<< 
Task<< 
DeleteProjectAsync<< (
(<<( )
Guid<<) -
	projectId<<. 7
)<<7 8
{== 
var>> 
project>> 
=>> 
await>> 
_projectRepository>> .
.>>. /!
GetWithWorkItemsAsync>>/ D
(>>D E
	projectId>>E N
)>>N O
;>>O P
if?? 

(?? 
project?? 
==?? 
null?? 
)?? 
{@@ 	
throwAA 
newAA  
KeyNotFoundExceptionAA *
(AA* +
$strAA+ >
)AA> ?
;AA? @
}BB 	
ifDD 

(DD 
projectDD 
.DD 
	WorkItemsDD 
.DD 
AnyDD !
(DD! "
wiDD" $
=>DD% '
wiDD( *
.DD* +
StatusDD+ 1
!=DD2 4
WorkItemStatusDD5 C
.DDC D
	CompletedDDD M
)DDM N
)DDN O
{EE 	
throwFF 
newFF %
InvalidOperationExceptionFF /
(FF/ 0
$strGG P
+GGQ R
$strHH A
)HHA B
;HHB C
}II 	
awaitKK 
_projectRepositoryKK  
.KK  !
DeleteAsyncKK! ,
(KK, -
projectKK- 4
)KK4 5
;KK5 6
awaitLL 
_unitOfWorkLL 
.LL 
CommitAsyncLL %
(LL% &
)LL& '
;LL' (
}MM 
}NN ¥
iC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Mappers\WorkItemMapper.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %
Mappers% ,
;, -
public 
static 
class 
WorkItemMapper "
{ 
public 

static 
WorkItemResponse "

ToResponse# -
(- .
this. 2
WorkItem3 ;
workItem< D
)D E
{		 
return

 
new

 
WorkItemResponse

 #
(

# $
workItem 
. 
Id 
, 
workItem 
. 
Title 
, 
workItem 
. 
Description  
,  !
workItem 
. 
DueDate 
, 
workItem 
. 
Status 
, 
workItem 
. 
Priority 
, 
workItem 
. 
	ProjectId 
, 
workItem 
. 
	CreatedBy 
) 
;  
} 
public 

static #
WorkItemDetailsResponse )
ToDetailsResponse* ;
(; <
this< @
WorkItemA I
workItemJ R
)R S
{ 
return 
new #
WorkItemDetailsResponse *
(* +
workItem 
. 
Id 
, 
workItem 
. 
Title 
, 
workItem 
. 
Description  
,  !
workItem 
. 
DueDate 
, 
workItem 
. 
Status 
, 
workItem 
. 
Priority 
, 
workItem 
. 
	ProjectId 
, 
workItem 
. 
	CreatedBy 
, 
workItem   
.   
Comments   
.   
Select   $
(  $ %
c  % &
=>  ' )
new  * -
CommentResponse  . =
(  = >
c!! 
.!! 
Id!! 
,!! 
c"" 
."" 
Content"" 
,"" 
c## 
.## 
AuthorId## 
,## 
c$$ 
.$$ 
	CreatedAt$$ 
)$$ 
)$$ 
,$$ 
workItem%% 
.%% 
History%% 
.%% 
Select%% #
(%%# $
h%%$ %
=>%%& (
new%%) ,
HistoryResponse%%- <
(%%< =
h&& 
.&& 
Id&& 
,&& 
h'' 
.'' 
Action'' 
,'' 
h(( 
.(( 
	Timestamp(( 
,(( 
h)) 
.)) 

ModifiedBy)) 
))) 
))) 
))) 
;))  
}** 
public,, 

static,, 
CommentResponse,, !

ToResponse,," ,
(,,, -
this,,- 1
WorkItemComment,,2 A
comment,,B I
),,I J
{-- 
return.. 
new.. 
CommentResponse.. "
(.." #
comment// 
.// 
Id// 
,// 
comment00 
.00 
Content00 
,00 
comment11 
.11 
AuthorId11 
,11 
comment22 
.22 
	CreatedAt22 
)22 
;22 
}33 
}44 á
hC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Mappers\ProjectMapper.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %
Mappers% ,
;, -
public		 
static		 
class		 
ProjectMapper		 !
{

 
public 

static 
ProjectResponse !

ToResponse" ,
(, -
this- 1
Project2 9
project: A
)A B
{ 
return 
new 
ProjectResponse "
(" #
project 
. 
Id 
, 
project 
. 
Name 
, 
project 
. 
Description 
,  
project 
. 
OwnerId 
, 
project 
. 
	WorkItems 
. 
Count #
)# $
;$ %
} 
public 

static "
ProjectDetailsResponse (
ToDetailsResponse) :
(: ;
this; ?
Project@ G
projectH O
)O P
{ 
return 
new "
ProjectDetailsResponse )
() *
project 
. 
Id 
, 
project 
. 
Name 
, 
project 
. 
Description 
,  
project 
. 
OwnerId 
, 
project 
. 
	WorkItems 
. 
Select $
($ %
wi% '
=>( *
wi+ -
.- .

ToResponse. 8
(8 9
)9 :
): ;
); <
;< =
} 
} Í
nC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Interfaces\IWorkItemService.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %

Interfaces% /
{ 
public 

	interface 
IWorkItemService %
{ 
Task 
< 
WorkItemResponse 
> 
CreateWorkItemAsync 2
(2 3!
CreateWorkItemRequest3 H
requestI P
)P Q
;Q R
Task		 
<		 #
WorkItemDetailsResponse		 $
>		$ %#
GetWorkItemDetailsAsync		& =
(		= >
Guid		> B

workItemId		C M
)		M N
;		N O
Task 
< 
WorkItemResponse 
> 
UpdateWorkItemAsync 2
(2 3
Guid3 7

workItemId8 B
,B C!
UpdateWorkItemRequestD Y
requestZ a
,a b
Guidc g

modifiedByh r
)r s
;s t
Task 
DeleteWorkItemAsync  
(  !
Guid! %

workItemId& 0
)0 1
;1 2
Task 
< 
CommentResponse 
> 
AddCommentAsync -
(- .
Guid. 2

workItemId3 =
,= >
AddCommentRequest? P
requestQ X
)X Y
;Y Z
} 
} §
lC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Interfaces\IReportService.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %
Services% -
;- .
public 
	interface 
IReportService 
{		 
Task 
< 	%
PerformanceReportResponse	 "
>" #*
GeneratePerformanceReportAsync$ B
(B C
GuidC G
userIdH N
,N O
DateTimeP X
fromDateY a
,a b
DateTimec k
toDatel r
)r s
;s t
} ì	
mC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\Interfaces\IProjectService.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %

Interfaces% /
{ 
public 

	interface 
IProjectService $
{		 
Task

 
<

 
ProjectResponse

 
>

 
CreateProjectAsync

 0
(

0 1 
CreateProjectRequest

1 E
request

F M
)

M N
;

N O
Task 
< 
IEnumerable 
< 
ProjectResponse (
>( )
>) *#
GetProjectsByOwnerAsync+ B
(B C
GuidC G
ownerIdH O
)O P
;P Q
Task 
< "
ProjectDetailsResponse #
># $"
GetProjectDetailsAsync% ;
(; <
Guid< @
	projectIdA J
)J K
;K L
Task 
DeleteProjectAsync 
(  
Guid  $
	projectId% .
). /
;/ 0
} 
} Í
dC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\DTOs\WorkItemDTOs.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %
DTOs% )
;) *
public 
record !
CreateWorkItemRequest #
(# $
string$ *
Title+ 0
,0 1
string2 8
Description9 D
,D E
DateTimeF N
DueDateO V
,V W
WorkItemPriority 
Priority 
, 
Guid #
	ProjectId$ -
,- .
Guid/ 3
	CreatedBy4 =
)= >
;> ?
public 
record !
UpdateWorkItemRequest #
(# $
string$ *
?* +
Title, 1
,1 2
string3 9
?9 :
Description; F
,F G
DateTimeH P
?P Q
DueDateR Y
,Y Z
WorkItemStatus 
? 
Status 
) 
; 
public		 
record		 
WorkItemResponse		 
(		 
Guid		 #
Id		$ &
,		& '
string		( .
Title		/ 4
,		4 5
string		6 <
Description		= H
,		H I
DateTime		J R
DueDate		S Z
,		Z [
WorkItemStatus

 
Status

 
,

 
WorkItemPriority

 +
Priority

, 4
,

4 5
Guid

6 :
	ProjectId

; D
,

D E
Guid

F J
	CreatedBy

K T
)

T U
;

U V
public 
record #
WorkItemDetailsResponse %
(% &
Guid& *
Id+ -
,- .
string/ 5
Title6 ;
,; <
string= C
DescriptionD O
,O P
DateTimeQ Y
DueDateZ a
,a b
WorkItemStatus 
Status 
, 
WorkItemPriority +
Priority, 4
,4 5
Guid6 :
	ProjectId; D
,D E
GuidF J
	CreatedByK T
,T U
IEnumerable 
< 
CommentResponse 
>  
Comments! )
,) *
IEnumerable+ 6
<6 7
HistoryResponse7 F
>F G
HistoryH O
)O P
;P Q
public 
record 
CommentResponse 
( 
Guid "
Id# %
,% &
string' -
Content. 5
,5 6
Guid7 ;
AuthorId< D
,D E
DateTimeF N
	CreatedAtO X
)X Y
;Y Z
public 
record 
HistoryResponse 
( 
Guid "
Id# %
,% &
string' -
Action. 4
,4 5
DateTime6 >
	Timestamp? H
,H I
GuidJ N

ModifiedByO Y
)Y Z
;Z [
public 
record 
AddCommentRequest 
(  
string  &
Content' .
,. /
Guid0 4
AuthorId5 =
)= >
;> ?–
bC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\DTOs\ReportDTOs.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %
DTOs% )
;) *
public 
record %
PerformanceReportResponse '
(' (
int( +
TotalCompleted, :
,: ;
double 
#
AverageCompletedPerUser "
," #
DateTime$ ,
FromDate- 5
,5 6
DateTime7 ?
ToDate@ F
)F G
;G H™

cC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\DTOs\ProjectDTOs.cs
	namespace 	
TaskManagement
 
. 
Application $
.$ %
DTOs% )
;) *
public 
record  
CreateProjectRequest "
(" #
string# )
Name* .
,. /
string0 6
Description7 B
,B C
GuidD H
OwnerIdI P
)P Q
;Q R
public 
record 
ProjectResponse 
( 
Guid "
Id# %
,% &
string' -
Name. 2
,2 3
string4 :
Description; F
,F G
GuidH L
OwnerIdM T
,T U
int 
WorkItemCount 
) 
; 
public		 
record		 "
ProjectDetailsResponse		 $
(		$ %
Guid		% )
Id		* ,
,		, -
string		. 4
Name		5 9
,		9 :
string		; A
Description		B M
,		M N
Guid

 
OwnerId

	 
,

 
IEnumerable

 
<

 
WorkItemResponse

 .
>

. /
	WorkItems

0 9
)

9 :
;

: ;ˆ	
fC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Application\DependencyInjection.cs
	namespace 	
TaskManagement
 
. 
Application $
{ 
public 

static 
class 
DependencyInjection +
{ 
public		 
static		 
IServiceCollection		 (-
!AddApplicationDependencyInjection		) J
(		J K
this		K O
IServiceCollection		P b
services		c k
)		k l
{

 	
services 
. 
	AddScoped 
< 
IProjectService .
,. /
ProjectService0 >
>> ?
(? @
)@ A
;A B
services 
. 
	AddScoped 
< 
IReportService -
,- .
ReportService/ <
>< =
(= >
)> ?
;? @
services 
. 
	AddScoped 
< 
IWorkItemService /
,/ 0
WorkItemService1 @
>@ A
(A B
)B C
;C D
return 
services 
; 
} 	
} 
} 