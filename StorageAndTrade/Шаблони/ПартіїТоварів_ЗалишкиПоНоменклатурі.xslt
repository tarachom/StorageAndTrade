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

					<h4>Партії товарів</h4>
					<p>
						На дату <xsl:value-of select="head/row/КінецьПеріоду"/><br/>
					    Номенклатура: <xsl:value-of select="head/row/Номенклатура"/>
					</p>
					
					<table class="table table-bordered table-sm table-hover">
						<tr class="table-success">
							<th width="10%" style="vertical-align:middle">Організація</th>
							<th width="25%" style="vertical-align:middle">Партія товарів</th>
							<th width="15%" style="vertical-align:middle">Характеристика</th>
							<th width="10%" style="vertical-align:middle">Серія</th>
							<th width="15%" style="vertical-align:middle">Склад</th>
							<th width="5%" style="text-align:center;vertical-align:middle">Кількість</th>
							<th width="5%" style="text-align:center;vertical-align:middle">Собівартість</th>
						</tr>

						<xsl:for-each select="ПартіїТоварів/row">
							<tr>
								<td>
									<a id="{Організація}" name="Довідник.Організації" href="/">
										<xsl:value-of select="Організація_Назва"/>
									</a>
								</td>
								<td>
									<a id="{ПартіяТоварівКомпозит}" name="Довідник.ПартіяТоварівКомпозит" href="/">
										<xsl:value-of select="ПартіяТоварівКомпозит_Назва"/>
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
								<td>
									<a id="{Склад}" name="Довідник.Склади" href="/">
										<xsl:value-of select="Склад_Назва"/>
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
