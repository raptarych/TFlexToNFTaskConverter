0</*! :: See https://github.com/ukoloff/dbs.js
@echo off
cscript.exe //Nologo //E:JScript "%~f0" %*
goto :EOF */0;
!function(n){function t(o){if(r[o])return r[o].X;var e=r[o]={X:{},id:o,loaded:!1};return n[o].call(e.X,e,e.X,t),
e.loaded=!0,e.X}var r={};return t.m=n,t.c=r,t.p="",t(0)}([function(n,t,r){r(77)(function(){r(111)})},function(n,t,r){
(function(t){n.X=t.h||WScript}).call(t,r(20))},function(n,t,r){(function(t,r){n.X=t(r)}).call(t,r(15),r(31));
},function(n,t,r){(function(t){n.X=function(n){return t.CreateObject(n)}}).call(t,r(1))},function(n,t){
n.X=function(n){var t;for(t=[],t._=0;n--;)t[n]=0;return t}},function(n,t,r){(function(t){n.X=t("Scripting.FileSystemObject");
}).call(t,r(3))},function(n,t,r){(function(t,r){n.X=t(r)}).call(t,r(15),r(32))},function(n,t,r){(function(t){
var r=[].slice;n.X=function(){var n;n=1<=arguments.length?r.call(arguments,0):[],t.Echo(n.join(" "))}}).call(t,r(1));
},function(n,t,r){(function(t,o,e,i){var u;u=r(8),n.X=function(n,r,u,c){var f,a;f=t(16),o(f,c),(a=3&f.length)&&o(f,t(4-a)),
e(f,a=f.length-4>>2),e(f,u),i(f,a),i(f,r),i(f,u),o(n,f)}}).call(t,r(4),r(27),r(12),r(22))},function(n,t,r){(function(t){
n.X=function(n){return t(n.z,n.a)}}).call(t,r(10))},function(n,t){n.X=function(n,t){return[n[0]-t[0],n[1]-t[1]];
}},function(n,t){var t,r,o;n.X=t=function(n){var t,r,e,i,u;for(u=o(arguments),e=u[0],r=u[1],i=0,t=new Enumerator(n);!t.atEnd();)r=e(t.item(),i++,r),
t.moveNext();return r},t.$=o=function(n,t){var o,e;for(null==t&&(t=1),e=2;e--;)if("function"==typeof(o=n[t+e]))return[o,n[t+1-e]];
return[r,[]]},r=function(n,t,r){return r.push(n),r}},function(n,t,r){var o;o=r(36),n.X=function(n,t,r){
var e;for(null==r&&(r=2),t>>>=0,e=o(n,r);r--;)n[e++]=255&t,t>>>=8}},function(n,t,r){(function(t){var r;n.X=function(n){
var o,e,i,u;for(u=t.$(arguments),e=u[0],o=u[1],i=0;n.length>i+1;)o=e({a:r(n[i]),b:n[i][2],z:r(n[i+1])},i,o),i++;
return o},r=function(n){return n.slice(0,2)}}).call(t,r(11))},function(n,t,r){var o;o=r(17),n.X=function(n){
return Math.sqrt(o(n))}},function(n,t,r){(function(t,r){var o,e;o=function(){},o.prototype=t,n.X=function(n){
var t,r,i,u;t=function(n){e(this,n)},t.prototype=u=new o;for(r in n)i=n[r],u[r]=i;return function(){return new t(arguments);
}},e=function(n,t){var o,e,i,u;for(i="",o=0,e=t.length;o<e;o++)u=t[o],i=r.BuildPath(i,u);n._=i}}).call(t,r(33),r(5));
},function(n,t,r){(function(t){n.X=function(n){if(!(n.length<2))return t(n[0],n[n.length-1])}}).call(t,r(40));
},function(n,t){n.X=function(n){return n[0]*n[0]+n[1]*n[1]}},function(n,t,r){(function(t){n.X=function(n){
null==n&&(n=0),t.Quit(n)}}).call(t,r(1))},function(n,t,r){(function(t,r){n.X=t(r.ScriptFullName)}).call(t,r(2),r(1));
},function(n,t){},,function(n,t,r){var o;o=r(12),n.X=function(n,t,r){null==r&&(r=2),o(n,t,r),n._+=r}},,function(n,t){
n.X=function(n,t){return t?[t[0][0]*n[0]+t[1][0]*n[1]+t[2][0],t[0][1]*n[0]+t[1][1]*n[1]+t[2][1]]:n.slice();
}},function(n,t,r){(function(t){n.X=function(n){var r;return r=t("ADODB.Stream"),r.Type=n?2:1,r.Open(),
r}}).call(t,r(3))},function(n,t,r){(function(n){var r;r=n("Msxml2.DOMDocument").createElement("tmp"),r.dataType="bin.hex",
t.enc=function(n){var t;return r.nodeTypedValue=n,t=r.text,r.text="",t},t.dec=function(n){var t;return r.text=n,
t=r.nodeTypedValue,r.text="",t}}).call(t,r(3))},function(n,t){n.X=function(n,t){n.push.apply(n,t)}},,,function(n,t,r){
var o,e;o=r(35),e=r(36),n.X=function(n,t){o.write(n,t,e(n,4),!0,23,4)}},function(n,t,r){(function(n){var r,o;
t.y=function(){return n.FileExists(this)},t.rm=function(t){return t&&!this.y()||n.DeleteFile(this),this},t.cp=function(t){
n.CopyFile(this,t)},t.mv=function(t){n.MoveFile(this,t)},t.$=function(){return n.GetFile(this)},t.age=function(n){
var t;return t=this.$().DateLastModified,null!=n?new Date-t>n:t},t.ok=function(n){return this.y()&&!this.age(n);
},o=function(t,r,o){var e,i,u,c,f;for(f="",i=u=0,c=o.length;u<c;i=++u)e=o[i],f+=",a["+i+"]";return new Function("x,f,a","return x."+t+"TextFile(f"+f+")")(n,r,o);
},t.open=function(){return o("Open",this,arguments)},t.create=function(){return o("Create",this,arguments)},t.load=function(){
var n,t;return n=this.open(1),t=n.ReadAll(),n.Close(),t},t.save=function(){var n,t,r,o;for(n=this.create(!0),
t=0,r=arguments.length;t<r;t++)o=arguments[t],n.Write(o);return n.Close()},r=function(n,t){var r,o,e,i;if(!n)return t;
for(e=[],r=0,o=t.length;r<o;r++)i=t[r],(i=i.replace(/^\s+|\s+$/g,""))&&e.push(i);return e},t.lines=function(n){
return r(n,this.load().split(/\r\n?|\n/))}}).call(t,r(5))},function(n,t,r){(function(n,o){var e;t.y=function(){
return n.FolderExists(this)},t.rm=function(t){return t&&!this.y()||n.DeleteFolder(this),this},t.cp=function(t){
n.CopyFolder(this,t)},t.mv=function(t){n.MoveFolder(this,t)},t.mk=function(t){var r,o,e;t&&this.rm(t),e=[],r=this._;
try{for(o=this.abs();!o.y();)e.push(o),o=o.up();for(;o=e.pop();)n.CreateFolder(o);return this}finally{this._=r;
}},t.$=function(){return n.GetFolder(this)},e=function(n,t,r){var e,i,u;return u=o.$(t,0),i=u[0],e=u[1],o(n,e,function(n,t,o){
return i(r(n),t,o)})},t.files=function(){return e(this.$().Files,arguments,r(2))},t.folders=function(){return e(this.$().SubFolders,arguments,r(6));
}}).call(t,r(5),r(11))},function(n,t,r){(function(n,o){t.toString=function(){var n;return null!=(n=this._)?n:"";
},t.abs=function(){return this._=n.GetAbsolutePathName(this),this},t.up=function(){var t;return(t=r(6))(n.GetParentFolderName(this));
},t.bn=function(){return n.GetFileName(this)},t.n=function(){return n.GetBaseName(this)},t.ext=function(){return n.GetExtensionName(this);
},t.ns=function(){return o.NameSpace(this._)}}).call(t,r(5),r(34))},function(n,t,r){(function(t){n.X=t("Shell.Application");
}).call(t,r(3))},function(n,t){t.read=function(n,t,r,o,e){var i,u,c=8*e-o-1,f=(1<<c)-1,a=f>>1,l=-7,s=r?e-1:0,h=r?-1:1,p=n[t+s];
for(s+=h,i=p&(1<<-l)-1,p>>=-l,l+=c;l>0;i=256*i+n[t+s],s+=h,l-=8);for(u=i&(1<<-l)-1,i>>=-l,l+=o;l>0;u=256*u+n[t+s],
s+=h,l-=8);if(0===i)i=1-a;else{if(i===f)return u?NaN:(p?-1:1)*(1/0);u+=Math.pow(2,o),i-=a}return(p?-1:1)*u*Math.pow(2,i-o);
},t.write=function(n,t,r,o,e,i){var u,c,f,a=8*i-e-1,l=(1<<a)-1,s=l>>1,h=23===e?Math.pow(2,-24)-Math.pow(2,-77):0,p=o?0:i-1,v=o?1:-1,x=t<0||0===t&&1/t<0?1:0;
for(t=Math.abs(t),isNaN(t)||t===1/0?(c=isNaN(t)?1:0,u=l):(u=Math.floor(Math.log(t)/Math.LN2),t*(f=Math.pow(2,-u))<1&&(u--,
f*=2),t+=u+s>=1?h/f:h*Math.pow(2,1-s),t*f>=2&&(u++,f/=2),u+s>=l?(c=0,u=l):u+s>=1?(c=(t*f-1)*Math.pow(2,e),u+=s):(c=t*Math.pow(2,s-1)*Math.pow(2,e),
u=0));e>=8;n[r+p]=255&c,p+=v,c/=256,e-=8);for(u=u<<e|c,a+=e;a>0;n[r+p]=255&u,p+=v,u/=256,a-=8);n[r+p-v]|=128*x;
}},function(n,t){n.X=function(n,t){var r;if(r=n._,n.length<(n._+=t))throw Error("Write beyond End of File");
return r}},,,function(n,t,r){(function(t,r){n.X=function(){var n,o,e,i,u,c,f,a;for(f=[[1,0],[0,1],[0,0]],
e=arguments.length-1;e>=0;e+=-1)if(a=arguments[e])for(o=i=f.length-1;i>=0;o=i+=-1)c=f[o],f[o]=t(c,a);for(n=f[2],
o=u=f.length-1;u>=0;o=u+=-1)c=f[o],2!==o&&(f[o]=r(c,n));return f}}).call(t,r(24),r(10))},function(n,t){n.X=function(n,t){
return n[0]===t[0]&&n[1]===t[1]}},function(n,t,r){(function(t){var o,e;e=r(9),o=function(n){return n*n},n.X=function(n){
var r,i;return i=(n.z[0]*n.a[1]-n.z[1]*n.a[0])/2,n.b&&(r=o(n.b),i-=(Math.atan(n.b)*o(1+r)-(1-r)*n.b)/r/8*t(e(n))),
i}}).call(t,r(17))},function(n,t,r){(function(t){var o;o=r(9),n.X=function(n){var r;return r=t(o(n)),n.b&&(r*=Math.atan(n.b)/n.b*(1+n.b*n.b)),
r}}).call(t,r(14))},,,function(n,t,r){(function(t){n.X=function(n){var r,o,e,i;for(i=0,e=n.paths,r=e.length-1;r>=0;r+=-1)o=e[r],
i+=t(o);return i}}).call(t,r(47))},function(n,t,r){(function(t){n.X=function(n){var r,o,e,i;for(o=0,i=n.paths,
r=i.length-1;r>=0;r+=-1)e=i[r],o+=t(e);return o}}).call(t,r(48))},function(n,t,r){(function(t){var o,e;o=r(16),
e=r(13),n.X=function(n){return o(n)?e(n,0,function(n,r,o){return o+t(n)}):0}}).call(t,r(41))},function(n,t,r){
(function(t){var o;o=r(13),n.X=function(n){return o(n,0,function(n,r,o){return o+t(n)})}}).call(t,r(42));
},,,,,,,,,,function(n,t,r){(function(t,r,o){var e;n.X=function(n,i){var u,c,f,a,l;for(a="",c=0,f=n.length;c<f;c++)u=n[c],
a+=e(u);l=t(),l.Write(r.dec(a));try{o(i).rm()}catch(n){}l.SaveToFile(i)},e=function(n){var t;for(t=n.toString(16);t.length<2;)t="0"+t;
return t.slice(-2)}}).call(t,r(25),r(26),r(2))},,,,,,,,,function(n,t,r){(function(t,r,o){n.X=function(n){
var e,i,u;for(e=t(4),i=u=0;u<=1;i=++u)r(e,-1);o(n,e)}}).call(t,r(4),r(12),r(27))},function(n,t,r){(function(t,o){
var e,i;i=r(69),e=r(67),n.X=function(n,r){var u,c,f,a;for(a=t(0),a.seq=0,u=0,c=n.length;u<c;u++)f=n[u],
i(a,f);e(a),o(a,r)}}).call(t,r(4),r(58))},function(n,t,r){var o,e,i,u;o=r(70),u=r(73),e=r(71),i=r(72),n.X=function(n,t){
var r,c,f,a,l,s,h;for(a=n.seq+t.paths.length+1,r=0,h=t.paths,l=c=0,f=h.length;c<f;l=++c)s=h[l],o(n,l?a:-a,s);n.seq=a,
u(n,t),e(n,t),i(n,t)}},function(n,t,r){(function(t,o,e,i,u){var c;c=r(8),n.X=function(n,r,f){var a,l,s,h,p,v,x,g,d,b,m,w,y;
for(a=t(48+12*f.length),o(a,1),a._=12,e(a,r),e(a,b=++n.seq),e(a,0),w=i(),l=0,v=w.length;l<v;l++)for(m=w[l],s=0,
x=m.length;s<x;s++)y=m[s],u(a,y);for(h=0,g=f.length;h<g;h++)for(m=f[h],p=0,d=m.length;p<d;p++)y=m[p],u(a,y);c(n,1,b,a);
}}).call(t,r(4),r(12),r(22),r(39),r(30))},function(n,t,r){(function(t){var o;o=r(8),n.X=function(n,r){var e,i,u;
for(e=t(8),i=u=0;u<=7;i=++u)e[i]=r.partid.charCodeAt(1^i)||32;o(n,26,n.seq,e)}}).call(t,r(4))},function(n,t,r){
(function(t,o,e,i){var u;u=r(8),n.X=function(n,r){var c;c=t(8),o(c,1e-4*e(r)),o(c,.01*i(r)),u(n,27,n.seq,c);
}}).call(t,r(4),r(30),r(45),r(46))},function(n,t,r){(function(t,o){var e;e=r(8),n.X=function(n,r){var i,u,c;
for(i=t(4*(c=r.paths.length)),u=n.seq-c;c--;)o(i,u++);e(n,8,n.seq,i)}}).call(t,r(4),r(22))},,function(n,t,r){
(function(t,r){n.X=t(r.Arguments)}).call(t,r(11),r(1))},function(n,t,r){(function(t){n.X=function(n){
var r,o,e,i,u,c,f;c={},i={};for(o in n)f=n[o],u={help:f},/^=/.test(f)&&(u={help:f.replace(/.\s*/,""),val:!0}),
c[o.charAt(0)]=i[o]=u;return e=function(n){return i[n].val?n+"[=?]":n},r=function(){var n;n=0;for(o in i)n=Math.max(n,e(o).length);
for(o in i){for(f=i[o],o=e(o);o.length<n;)o+=" ";t("  -"+o.charAt(0)+"  --"+o+" "+f.help)}t("\nSee","https://github.com/ukoloff/dbs.js");
},function(n){var t,o,e,u,f,a,l,s;if(!n)return void r();for(n=n.slice(),a=[],u=function(){var t;return a[o]=null==(t=n.shift())||t;
},e=function(){return a[o]||(a[o]=0),a[o]++};n.length;){if("--"===(t=n.shift())){a.push.apply(a,n);break}if(/^--\w/.test(t)){
if(t=t.substring(2),(s=/=/.test(t))&&(t=RegExp.leftContext,l=RegExp.rightContext),!(f=i[t]))throw Error("Unknown option: --"+t);
o=t.charAt(0),s?a[o]=l:f.val?u():e()}else if(/^-\w/.test(t))for(t=t.substring(1);t.length;){if(!(f=c[o=t.charAt(0).toLowerCase()]))throw Error("Unknown option: -"+o);
if(t=t.substring(1),f.val){t?a[o]=t:u();break}e()}else a.push(t)}return a}}}).call(t,r(7))},function(n,t,r){n.X=function(n){
var t;try{n()}catch(n){if(t=n,!t.message)throw t;r(7)("ERROR:",t.message)}}},,,,,,,,,,,,,,,,,,,function(n,t){
n.X=function(n){if(n+="",/[^{\[:\s,\]}]/.test(n.replace(/-?\d+(?:[.]\d*)?(?:[eE][-+]?\d+)?|"(?:\\.|[^"\\])*"|\b(?:null|true|false)\b/g,"]")))throw SyntaxError("Invalid JSON");
return new Function("return ("+n+")")()}},,,,,,,,,,,,,,,function(n,t,r){(function(n,t,o,e,i,u,c,f,a,l){var s,h,p,v,x;
v=n(r(170)),x=v(t),(x.h||1!==x.length&&!x.s)&&(o("Usage: "+e.bn()+" [options] path/to/file.json\n\nOptions:"),
v(),i()),x.s?(x[0]="-",p=u.StdIn.ReadAll()):(o("Reading",x[0]),p=c(x[0]).load()),p=f(p),h=x[0],x.o&&(h=(s=a(x.o)).y()?c(s,c(h).bn()):x.o),
h=c(h+".dbs").abs(),!x.f&&h.y()&&(o("Skipping:",h),i(1)),o("Writing:",h),l(p,h)}).call(t,r(76),r(75),r(7),r(19),r(18),r(1),r(2),r(96),r(6),r(68));
},,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,function(n,t){n.X={"help":"Show this help",
"output":"=Save result to file/folder","force":"Overwrite existing file","stdin":"Read from stdin"}}]);
