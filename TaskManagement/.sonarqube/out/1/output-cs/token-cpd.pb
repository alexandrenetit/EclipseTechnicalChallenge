Å
fC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Services\WorkItemService.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Services  (
;( )
public

 
class

 !
WorkItemServiceDomain

 "
:

# $"
IWorkItemServiceDomain

% ;
{ 
public 

void $
ValidateWorkItemCreation (
(( )
Project) 0
project1 8
,8 9
WorkItemPriority: J
priorityK S
)S T
{ 
if 

( 
project 
. 
	WorkItems 
. 
Count #
>=$ &
$num' )
)) *
{ 	
throw 
new 
DomainException %
(% &
$str& S
)S T
;T U
} 	
if 

( 
priority 
== 
WorkItemPriority (
.( )
High) -
&&. 0
project 
. 
	WorkItems 
. 
Count #
(# $
i$ %
=>& (
i) *
.* +
Priority+ 3
==4 6
WorkItemPriority7 G
.G H
HighH L
)L M
>=N P
$numQ R
)R S
{ 	
throw 
new 
DomainException %
(% &
$str& `
)` a
;a b
} 	
} 
} °
mC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Services\IWorkItemServiceDomain.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Services  (
;( )
public		 
	interface		 "
IWorkItemServiceDomain		 '
{

 
void $
ValidateWorkItemCreation	 !
(! "
Project" )
project* 1
,1 2
WorkItemPriority3 C
priorityD L
)L M
;M N
} ‘	
nC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Repositories\IWorkItemRepository.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Repositories  ,
;, -
public 
	interface 
IWorkItemRepository $
:% &
IRepository' 2
<2 3
WorkItem3 ;
>; <
{		 
Task

 
<

 	
IEnumerable

	 
<

 
WorkItem

 
>

 
>

 
GetByProjectIdAsync

  3
(

3 4
Guid

4 8
	projectId

9 B
)

B C
;

C D
Task 
< 	
WorkItem	 
? 
> 
GetWithDetailsAsync '
(' (
Guid( ,
id- /
)/ 0
;0 1
Task 
AddCommentAsync	 
( 
WorkItemComment (
comment) 0
)0 1
;1 2
Task 
AddHistoryAsync	 
( 
WorkItemHistory (
history) 0
)0 1
;1 2
} °
jC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Repositories\IUserRepository.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Repositories  ,
;, -
public 
	interface 
IUserRepository  
:! "
IRepository# .
<. /
User/ 3
>3 4
{ 
} ú
fC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Repositories\IUnitOfWork.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Repositories  ,
{ 
public 

	interface 
IUnitOfWork  
:! "
IDisposable# .
{ 
IProjectRepository 
Projects #
{$ %
get& )
;) *
}+ ,
IWorkItemRepository		 
	WorkItems		 %
{		& '
get		( +
;		+ ,
}		- .
Task 
< 
int 
> 
CommitAsync 
( 
) 
;  
} 
} ä	
fC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Repositories\IRepository.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Repositories  ,
{ 
public 

	interface 
IRepository  
<  !
T! "
>" #
where$ )
T* +
:, -
Entity. 4
<4 5
Guid5 9
>9 :
{		 
Task

 
<

 
T

 
?

 
>

 
GetByIdAsync

 
(

 
Guid

 "
id

# %
)

% &
;

& '
Task 
< 
IEnumerable 
< 
T 
> 
> 
GetAllAsync (
(( )
)) *
;* +
Task 
AddAsync 
( 
T 
entity 
) 
;  
Task 
UpdateAsync 
( 
T 
entity !
)! "
;" #
Task 
DeleteAsync 
( 
T 
entity !
)! "
;" #
} 
} ÿ
mC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Repositories\IProjectRepository.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Repositories  ,
{ 
public 

	interface 
IProjectRepository '
:( )
IRepository* 5
<5 6
Project6 =
>= >
{		 
Task

 
<

 
IEnumerable

 
<

 
Project

  
>

  !
>

! "
GetByOwnerIdAsync

# 4
(

4 5
Guid

5 9
ownerId

: A
)

A B
;

B C
Task 
< 
Project 
? 
> !
GetWithWorkItemsAsync ,
(, -
Guid- 1
id2 4
)4 5
;5 6
} 
} ¤
hC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Exceptions\DomainException.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  

Exceptions  *
;* +
public 
class 
DomainException 
: 
	Exception (
{ 
public 

DomainException 
( 
) 
{		 
}		 
public 

DomainException 
( 
string !
message" )
)) *
: 	
base
 
( 
message 
) 
{ 
} 
public 

DomainException 
( 
string !
message" )
,) *
	Exception+ 4
innerException5 C
)C D
: 	
base
 
( 
message 
, 
innerException &
)& '
{ 
} 
} ¥
bC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Enums\WorkItemStatus.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Enums  %
;% &
public 
enum 
WorkItemStatus 
{ 
Pending 
, 

InProgress		 
,		 
	Completed

 
} œ
dC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Enums\WorkItemPriority.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Enums  %
;% &
public 
enum 
WorkItemPriority 
{ 
Low 
, 
Medium		 

,		
 
High

 
} ±
fC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Entities\WorkItemHistory.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Entities  (
;( )
public 
class 
WorkItemHistory 
: 
Entity %
<% &
Guid& *
>* +
{		 
public

 

string

 
Action

 
{

 
get

 
;

 
private

  '
set

( +
;

+ ,
}

- .
public 

DateTime 
	Timestamp 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
public 

Guid 

ModifiedBy 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
public 

Guid 

WorkItemId 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
public 

WorkItemHistory 
( 
) 
{ 
} 
public 

WorkItemHistory 
( 
Guid 
id  "
," #
string$ *
action+ 1
,1 2
DateTime3 ;
	timestamp< E
,E F
GuidG K

modifiedByL V
,V W
GuidX \

workItemId] g
)g h
{ 
Id 

= 
id 
; 
Action 
= 
action 
; 
	Timestamp 
= 
	timestamp 
; 

ModifiedBy 
= 

modifiedBy 
;  

WorkItemId 
= 

workItemId 
;  
} 
} “
fC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Entities\WorkItemComment.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Entities  (
;( )
public 
class 
WorkItemComment 
: 
Entity %
<% &
Guid& *
>* +
{		 
public

 

string

 
Content

 
{

 
get

 
;

  
private

! (
set

) ,
;

, -
}

. /
public 

Guid 
AuthorId 
{ 
get 
; 
private  '
set( +
;+ ,
}- .
public 

Guid 

WorkItemId 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
public 

DateTime 
	CreatedAt 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
public 

WorkItemComment 
( 
) 
{ 
} 
public 

WorkItemComment 
( 
Guid 
id  "
," #
string$ *
content+ 2
,2 3
Guid4 8
authorId9 A
,A B
GuidC G

workItemIdH R
)R S
{ 
Id 

= 
id 
; 
Content 
= 
content 
; 
AuthorId 
= 
authorId 
; 

WorkItemId 
= 

workItemId 
;  
	CreatedAt 
= 
DateTime 
. 
UtcNow #
;# $
} 
} žB
_C:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Entities\WorkItem.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Entities  (
;( )
public		 
class		 
WorkItem		 
:		 
Entity		 
<		 
Guid		 #
>		# $
{

 
public 

string 
Title 
{ 
get 
; 
private &
set' *
;* +
}, -
public 

string 
Description 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
public 

DateTime 
DueDate 
{ 
get !
;! "
private# *
set+ .
;. /
}0 1
public 

WorkItemStatus 
Status  
{! "
get# &
;& '
private( /
set0 3
;3 4
}5 6
public 

WorkItemPriority 
Priority $
{% &
get' *
;* +
private, 3
init4 8
;8 9
}: ;
public 

Guid 
	ProjectId 
{ 
get 
;  
private! (
set) ,
;, -
}. /
public 

Guid 
	CreatedBy 
{ 
get 
;  
private! (
set) ,
;, -
}. /
private 
readonly 
List 
< 
WorkItemComment )
>) *
	_comments+ 4
=5 6
new7 :
(: ;
); <
;< =
public 

IReadOnlyCollection 
< 
WorkItemComment .
>. /
Comments0 8
=>9 ;
	_comments< E
.E F

AsReadOnlyF P
(P Q
)Q R
;R S
private 
readonly 
List 
< 
WorkItemHistory )
>) *
_history+ 3
=4 5
new6 9
(9 :
): ;
;; <
public 

IReadOnlyCollection 
< 
WorkItemHistory .
>. /
History0 7
=>8 :
_history; C
.C D

AsReadOnlyD N
(N O
)O P
;P Q
private 
WorkItem 
( 
) 
{ 
} 
public 

WorkItem 
( 
Guid 
id 
, 
string 
title 
, 
string 
description 
, 
DateTime   
dueDate   
,   
WorkItemPriority!! 
priority!! !
,!!! "
Guid"" 
	projectId"" 
,"" 
Guid## 
	createdBy## 
)## 
{$$ 
Id%% 

=%% 
id%% 
;%% 
Title&& 
=&& 
title&& 
;&& 
Description'' 
='' 
description'' !
;''! "
DueDate(( 
=(( 
dueDate(( 
;(( 
Priority)) 
=)) 
priority)) 
;)) 
	ProjectId** 
=** 
	projectId** 
;** 
	CreatedBy++ 
=++ 
	createdBy++ 
;++ 
Status,, 
=,, 
WorkItemStatus,, 
.,,  
Pending,,  '
;,,' (
RecordHistory.. 
(.. 
$str.. )
)..) *
;..* +
}// 
public11 

void11 
UpdateStatus11 
(11 
WorkItemStatus11 +
	newStatus11, 5
,115 6
Guid117 ;

modifiedBy11< F
)11F G
{22 
if33 

(33 
Status33 
==33 
	newStatus33 
)33  
return33! '
;33' (
Status55 
=55 
	newStatus55 
;55 
RecordHistory66 
(66 
$"66 
$str66 *
{66* +
	newStatus66+ 4
}664 5
"665 6
,666 7

modifiedBy668 B
)66B C
;66C D
}77 
public99 

void99 
UpdateDetails99 
(99 
string99 $
title99% *
,99* +
string99, 2
description993 >
,99> ?
DateTime99@ H
dueDate99I P
,99P Q
Guid99R V

modifiedBy99W a
)99a b
{:: 
var;; 
changes;; 
=;; 
new;; 
List;; 
<;; 
string;; %
>;;% &
(;;& '
);;' (
;;;( )
if== 

(== 
Title== 
!=== 
title== 
)== 
{>> 	
changes?? 
.?? 
Add?? 
(?? 
$"?? 
$str?? .
{??. /
Title??/ 4
}??4 5
$str??5 ;
{??; <
title??< A
}??A B
$str??B C
"??C D
)??D E
;??E F
Title@@ 
=@@ 
title@@ 
;@@ 
}AA 	
ifCC 

(CC 
DescriptionCC 
!=CC 
descriptionCC &
)CC& '
{DD 	
changesEE 
.EE 
AddEE 
(EE 
$strEE -
)EE- .
;EE. /
DescriptionFF 
=FF 
descriptionFF %
;FF% &
}GG 	
ifII 

(II 
DueDateII 
!=II 
dueDateII 
)II 
{JJ 	
changesKK 
.KK 
AddKK 
(KK 
$"KK 
$strKK 0
{KK0 1
DueDateKK1 8
:KK8 9
$strKK9 C
}KKC D
$strKKD H
{KKH I
dueDateKKI P
:KKP Q
$strKKQ [
}KK[ \
"KK\ ]
)KK] ^
;KK^ _
DueDateLL 
=LL 
dueDateLL 
;LL 
}MM 	
ifOO 

(OO 
changesOO 
.OO 
AnyOO 
(OO 
)OO 
)OO 
{PP 	
RecordHistoryQQ 
(QQ 
stringQQ  
.QQ  !
JoinQQ! %
(QQ% &
$strQQ& *
,QQ* +
changesQQ, 3
)QQ3 4
,QQ4 5

modifiedByQQ6 @
)QQ@ A
;QQA B
}RR 	
}SS 
publicUU 

voidUU 

AddCommentUU 
(UU 
stringUU !
contentUU" )
,UU) *
GuidUU+ /
authorIdUU0 8
)UU8 9
{VV 
varWW 
commentWW 
=WW 
newWW 
WorkItemCommentWW )
(WW) *
GuidWW* .
.WW. /
NewGuidWW/ 6
(WW6 7
)WW7 8
,WW8 9
contentWW: A
,WWA B
authorIdWWC K
,WWK L
IdWWM O
)WWO P
;WWP Q
	_commentsXX 
.XX 
AddXX 
(XX 
commentXX 
)XX 
;XX 
RecordHistoryYY 
(YY 
$"YY 
$strYY '
{YY' (
contentYY( /
}YY/ 0
"YY0 1
,YY1 2
authorIdYY3 ;
)YY; <
;YY< =
}ZZ 
private\\ 
void\\ 
RecordHistory\\ 
(\\ 
string\\ %
action\\& ,
,\\, -
Guid\\. 2
?\\2 3

modifiedBy\\4 >
=\\? @
null\\A E
)\\E F
{]] 
_history^^ 
.^^ 
Add^^ 
(^^ 
new^^ 
WorkItemHistory^^ (
(^^( )
Guid__ 
.__ 
NewGuid__ 
(__ 
)__ 
,__ 
action`` 
,`` 
DateTimeaa 
.aa 
UtcNowaa 
,aa 

modifiedBybb 
??bb 
	CreatedBybb #
,bb# $
Idcc 
)cc 
)cc 
;cc 
}dd 
}ee Ð
[C:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Entities\User.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Entities  (
;( )
public 
class 
User 
: 
Entity 
< 
Guid 
>  
{		 
public

 

string

 
Name

 
{

 
get

 
;

 
private

 %
set

& )
;

) *
}

+ ,
public 

string 
Email 
{ 
get 
; 
private &
set' *
;* +
}, -
public 

bool 
	IsManager 
{ 
get 
;  
private! (
set) ,
;, -
}. /
private 
User 
( 
) 
{ 
} 
public 

User 
( 
Guid 
id 
, 
string 
name  $
,$ %
string& ,
email- 2
,2 3
bool4 8
	isManager9 B
=C D
falseE J
)J K
{ 
Id 

= 
id 
; 
Name 
= 
name 
; 
Email 
= 
email 
; 
	IsManager 
= 
	isManager 
; 
} 
} À
^C:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Entities\Project.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Entities  (
;( )
public

 
class

 
Project

 
:

 
Entity

 
<

 
Guid

 "
>

" #
{ 
public 

string 
Name 
{ 
get 
; 
private %
set& )
;) *
}+ ,
public 

string 
Description 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
public 

Guid 
OwnerId 
{ 
get 
; 
private &
set' *
;* +
}, -
public 

virtual 
User 
Owner 
{ 
get  #
;# $
private% ,
set- 0
;0 1
}2 3
private 
readonly 
List 
< 
WorkItem "
>" #

_workItems$ .
=/ 0
new1 4
(4 5
)5 6
;6 7
public 

IReadOnlyCollection 
< 
WorkItem '
>' (
	WorkItems) 2
=>3 5

_workItems6 @
.@ A

AsReadOnlyA K
(K L
)L M
;M N
private 
readonly 
List 
< 
ProjectMember '
>' (
_members) 1
=2 3
new4 7
(7 8
)8 9
;9 :
public 

IReadOnlyCollection 
< 
ProjectMember ,
>, -
Members. 5
=>6 8
_members9 A
.A B

AsReadOnlyB L
(L M
)M N
;N O
private 
Project 
( 
) 
{ 
} 
public 

Project 
( 
Guid 
id 
, 
string "
name# '
,' (
string) /
description0 ;
,; <
Guid= A
ownerIdB I
)I J
{   
Id!! 

=!! 
id!! 
;!! 
Name"" 
="" 
name"" 
;"" 
Description## 
=## 
description## !
;##! "
OwnerId$$ 
=$$ 
ownerId$$ 
;$$ 
}%% 
public'' 

void'' 
AddWorkItem'' 
('' 
WorkItem'' $
workItem''% -
)''- .
{(( 
if)) 

()) 

_workItems)) 
.)) 
Count)) 
>=)) 
$num))  "
)))" #
throw** 
new** 
DomainException** %
(**% &
$str**& S
)**S T
;**T U

_workItems,, 
.,, 
Add,, 
(,, 
workItem,, 
),,  
;,,  !
}-- 
public// 

void// 
RemoveWorkItem// 
(// 
WorkItem// '
workItem//( 0
)//0 1
{00 
if11 

(11 
workItem11 
.11 
Status11 
!=11 
WorkItemStatus11 -
.11- .
	Completed11. 7
)117 8
throw22 
new22 
DomainException22 %
(22% &
$str22& W
)22W X
;22X Y

_workItems44 
.44 
Remove44 
(44 
workItem44 "
)44" #
;44# $
}55 
}66 ð
dC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Entities\ProjectMember.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Entities  (
{ 
public 

class 
ProjectMember 
:  
Entity! '
<' (
Guid( ,
>, -
{		 
public

 
Guid

 
	ProjectId

 
{

 
get

  #
;

# $
private

% ,
set

- 0
;

0 1
}

2 3
public 
Guid 
UserId 
{ 
get  
;  !
private" )
set* -
;- .
}/ 0
public 
DateTime 
JoinedAt  
{! "
get# &
;& '
private( /
set0 3
;3 4
}5 6
private 
ProjectMember 
( 
) 
{ 	
}
 
public 
ProjectMember 
( 
Guid !
	projectId" +
,+ ,
Guid- 1
userId2 8
)8 9
{ 	
	ProjectId 
= 
	projectId !
;! "
UserId 
= 
userId 
; 
JoinedAt 
= 
DateTime 
.  
UtcNow  &
;& '
} 	
} 
} Õ#
bC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\Entities\Base\Entity.cs
	namespace 	
TaskManagement
 
. 
Domain 
.  
Entities  (
.( )
Base) -
;- .
public 
abstract 
class 
Entity 
< 
T 
> 
where  %
T& '
:( )
notnull* 1
{ 
public 

T 
Id 
{ 
get 
; 
	protected  
set! $
;$ %
}& '
public 

DateTime 
	CreatedAt 
{ 
get  #
;# $
	protected% .
set/ 2
;2 3
}4 5
public 

DateTime 
? 
	UpdatedAt 
{  
get! $
;$ %
	protected& /
set0 3
;3 4
}5 6
	protected		 
Entity		 
(		 
)		 
{

 
	CreatedAt 
= 
DateTime 
. 
UtcNow #
;# $
} 
	protected 
Entity 
( 
T 
id 
) 
: 
this !
(! "
)" #
{ 
Id 

= 
id 
; 
} 
public 

void 
MarkAsUpdated 
( 
) 
{ 
	UpdatedAt 
= 
DateTime 
. 
UtcNow #
;# $
} 
public 

override 
bool 
Equals 
(  
object  &
?& '
obj( +
)+ ,
{ 
if 

( 
obj 
is 
not 
Entity 
< 
T 
>  
other! &
)& '
return 
false 
; 
if 

( 
ReferenceEquals 
( 
this  
,  !
other" '
)' (
)( )
return 
true 
; 
if!! 

(!! 
GetType!! 
(!! 
)!! 
!=!! 
other!! 
.!! 
GetType!! &
(!!& '
)!!' (
)!!( )
return"" 
false"" 
;"" 
return$$ 
Id$$ 
.$$ 
Equals$$ 
($$ 
other$$ 
.$$ 
Id$$ !
)$$! "
;$$" #
}%% 
public'' 

static'' 
bool'' 
operator'' 
==''  "
(''" #
Entity''# )
<'') *
T''* +
>''+ ,
?'', -
left''. 2
,''2 3
Entity''4 :
<'': ;
T''; <
>''< =
?''= >
right''? D
)''D E
{(( 
if)) 

()) 
left)) 
is)) 
null)) 
&&)) 
right)) !
is))" $
null))% )
)))) *
return** 
true** 
;** 
if,, 

(,, 
left,, 
is,, 
null,, 
||,, 
right,, !
is,," $
null,,% )
),,) *
return-- 
false-- 
;-- 
return// 
left// 
.// 
Equals// 
(// 
right//  
)//  !
;//! "
}00 
public22 

static22 
bool22 
operator22 
!=22  "
(22" #
Entity22# )
<22) *
T22* +
>22+ ,
?22, -
left22. 2
,222 3
Entity224 :
<22: ;
T22; <
>22< =
?22= >
right22? D
)22D E
{33 
return44 
!44 
(44 
left44 
==44 
right44 
)44 
;44  
}55 
public77 

override77 
int77 
GetHashCode77 #
(77# $
)77$ %
{88 
return99 
HashCode99 
.99 
Combine99 
(99  
Id99  "
,99" #
	CreatedAt99$ -
)99- .
;99. /
}:: 
};; Æ
aC:\Labs\EclipseTechnicalChallenge\TaskManagement\src\TaskManagement.Domain\DependencyInjection.cs
	namespace 	
TaskManagement
 
. 
Domain 
;  
public 
static 
class 
DependencyInjection '
{ 
public 

static 
IServiceCollection $(
AddDomainDependencyInjection% A
(A B
thisB F
IServiceCollectionG Y
servicesZ b
)b c
{		 
services

 
.

 
	AddScoped

 
<

 "
IWorkItemServiceDomain

 1
,

1 2!
WorkItemServiceDomain

3 H
>

H I
(

I J
)

J K
;

K L
return 
services 
; 
} 
} 