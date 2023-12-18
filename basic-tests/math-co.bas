10 ' Do a large multiplication
20 pokew $de00,6789
30 pokew $de02,4390

40 ' The result should be 29,803,710
50 print peekd($de10)

100 ' Do a large division
110 pokew $de06,48763   ' numerator
120 pokew $de04,189     ' denominator

130 ' The result should be 258, with remainder 1
140 print "quotient:", peekw($de14)
150 print "remainder:", peekw($de16)

200 ' Do a large addition
210 poked $de08,890789078
220 poked $de0c,123123123

230 ' The result is 1,013,912,201
240 print peekd($de18)