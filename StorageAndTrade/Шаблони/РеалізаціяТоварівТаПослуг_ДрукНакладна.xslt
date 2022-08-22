<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:output method="html" indent="yes" doctype-system="html" />

	<xsl:template name="Head">
		<meta charset="utf-8" />
		<meta name="viewport" content="width=device-width, initial-scale=1" />
		<link rel="stylesheet" href="Style/bootstrap.min.css" />
	</xsl:template>

    <xsl:template match="/root">

		<html>
			<head>
				<title>Table</title>
				<xsl:call-template name="Head" />
			</head>
			<body>

				<div class="container-fluid">

					<h4>Накладна</h4>
					
					<table class="table table-sm">
						<xsl:for-each select="row">
							<tr class="table-info">
								<th>Документ:</th>
								<td><xsl:value-of select="Назва"/></td>
							</tr>
							<tr>
								<th>Дата:</th>
								<td><xsl:value-of select="ДатаДок"/></td>
							</tr>
							<tr>	
								<th>Номер:</th>
								<td><xsl:value-of select="НомерДок"/></td>
							</tr>
						</xsl:for-each>
					</table>
					
					<table class="table table-bordered table-sm table-hover">
						<tr class="table-success">
							<th>Номенклатура</th>
							<th>Характеристика</th>
							<th>Серія</th>
							<th style="text-align:center">Кількість</th>
							<th style="text-align:center">Ціна</th>
						    <th style="text-align:center">Сума</th>
						</tr>

						<xsl:for-each select="ПартіїТоварів/row">
							<tr>
								<td>
									<xsl:value-of select="Номенклатура_Назва"/>
								</td>
								<td>
									<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
								</td>
								<td>
									<xsl:value-of select="Серія_Номер"/>
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
