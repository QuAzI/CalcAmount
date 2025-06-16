# Calculate amount assignment  

## 1.  We are going to use https://www.frankfurter.app/  to fetch currency rates 

## 2.  Go to the documentation https://www.frankfurter.app/docs/#historical 
 
## 3.  Create a form that will let the user input: 
●  Amount in EUR 
●  3 currencies (for example: USD, ILS, CAD) 
●  Currencies can be a drop down list of predefi ned currencies  
 
## 4.  Clicking the Submit button will perform an ajax request to the server to get the EUR rates for 
these currencies of every Monday for the past 7 weeks, Starti ng today. 
We expect to get 7 rows. 
Note 

## 5.  Display the calculated amount for every week in each currency. 

For every currency, the highest amount will be marked green and the lowest in red. 
Result should be “like” this 
 
Display a grid with the results, if possible with a fade in/scroll down eff ect. 
a.  The grid should be centered in the screen 
b.  The header should be in a diff erent color then the body. 
c.  Data rows should also get a diff erent color and should be alternati ng.  

## 6.  Decide on a scenario that will throw an excepti on at the server 

(like specific amount/invalid currency) 
and show the error in a popup/div at the client 

## 7.  Try to make your solution efficient

if a user requests to change the amount  – don’t get rates you already requested from the api 

## 8.  Make it an MVC app

## 9.  At the client side use plain javascript+jQuery/underscore

(no other frameworks/libraries like Angular/react..)
