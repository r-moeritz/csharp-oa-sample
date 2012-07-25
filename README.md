csharp-oa-sample
================

This is a sample solution, in C#, to the expression problem using
object algebras, as described
[here](http://ropas.snu.ac.kr/~bruno/papers/ecoop2012.pdf) [PDF]. I've
tried to keep this as close to the Java solution as possible, but a
few changes were unavoidable or desireable:

 * Anonymous classes in C# aren't as powerful as their Java
   counterparts. Specifically, they can't implement interfaces, so
   it was necessary to use Castle Dynamic Proxy to generate proxy
   objects from delegates. This added some complexity, but I've swept
   it under the carpet so as not to spoil the readability of the solution
   proper.
 * Lowercase method names are not considered idiomatic C# style, so
   I've taken the liberty of uppercasing them.
