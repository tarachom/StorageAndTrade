<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" doctype-system="html" />

	<xsl:template name="Head">
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="Style/bootstrap.min.css" />
		<script src="Style/jquery.slim.min.js"></script>
		<script src="Style/popper.min.js"></script>
		<script src="Style/bootstrap.bundle.min.js"></script>
	</xsl:template>

    <xsl:template match="/root">

		<html>
			<head>
				<title>Table</title>
				<xsl:call-template name="Head" />
			</head>
			<body>

				<div class="container-fluid">

					<h4>Партії товарів</h4>
					<p>
						На дату <xsl:value-of select="head/row/КінецьПеріоду"/><br/>
					    Номенклатура: <xsl:value-of select="head/row/Номенклатура"/>
					</p>
					
					<table class="table table-bordered table-sm table-hover">
						<tr class="table-success">
							<th>Організація</th>
							<th>Документ поступлення</th>
							<th>Характеристика</th>
							<th>Серія</th>
							<th style="text-align:center">Кількість</th>
							<th style="text-align:center">Собівартість</th>
						</tr>

						<xsl:for-each select="ПартіїТоварів/row">
							<tr>
								<td>
									<a id="{Організація}" name="Довідник.Організації" href="/">
										<xsl:value-of select="Організація_Назва"/>
									</a>
								</td>
								<td>
									<a id="{ДокументПоступлення}" name="Документ.ПоступленняТоварівТаПослуг" href="/">
										<xsl:value-of select="ДокументПоступлення_Назва"/>
									</a>
								</td>
								<td>
									<a id="{ХарактеристикаНоменклатури}" name="Довідник.Характеристика" href="/">
										<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
									</a>
								</td>
								<td>
									<a id="{Серія}" name="Довідник.СеріїНоменклатури" href="/">
										<xsl:value-of select="Серія_Номер"/>
									</a>
								</td>
								<td align="right">
									<xsl:value-of select="Кількість"/>
								</td>
								<td align="right">
									<xsl:value-of select="Собівартість"/>
								</td>
							</tr>
						</xsl:for-each>

					</table>

					<br/>
					<br/>
					<br/>
				    <br/>
				
				</div>

			</body>
		</html>
				
    </xsl:template>
	
</xsl:stylesheet>
