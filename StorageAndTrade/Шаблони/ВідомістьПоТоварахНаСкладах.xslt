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

				<div class="container">

					<br/>
					<h2>Відомість по товарах на складах</h2>
					<br/>

					<table class="table table-bordered table-sm">
						<tr class="table-success">
							<th>Склад</th>
							<th>Номенклатура</th>
							<th>Характеристика</th>
							<th>Початковий</th>
							<th>Прихід</th>
							<th>Розхід</th>
							<th>Кінцевий</th>
						</tr>

						<xsl:for-each select="ВідомістьПоТоварахНаСкладах/row">
							<tr>
								<td>
									<xsl:value-of select="Склад_Назва"/>
								</td>
								<td>
									<xsl:value-of select="Номенклатура_Назва"/>
								</td>
								<td>
									<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
								</td>
								<td align="right">
									<xsl:value-of select="ПочатковийЗалишок"/>
								</td>
								<td align="right">
									<xsl:value-of select="Прихід"/>
								</td>
								<td align="right">
									<xsl:value-of select="Розхід"/>
								</td>
								<td align="right">
									<xsl:value-of select="КінцевийЗалишок"/>
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
