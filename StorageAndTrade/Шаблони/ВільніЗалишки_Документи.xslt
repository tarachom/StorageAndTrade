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

					<h4>Документи</h4>
					<p>
						Період з <xsl:value-of select="head/row/ПочатокПеріоду"/> по <xsl:value-of select="head/row/КінецьПеріоду"/>
					</p>

					<table class="table table-bordered table-sm table-hover">
						<tr class="table-success">
							<th>Документ</th>
							<th>Номенклатура</th>
							<th>Характеристика</th>
							<th>Склад</th>
							<th></th>
							<th style="text-align:center">ВРезервіЗіСкладу</th>
							<th style="text-align:center">ВРезервіПідЗамовлення</th>
						</tr>

						<xsl:for-each select="Документи/row">
							<tr>
								<td>
									<a id="{uid}" name="Документ.{doctype}" href="/">
										<xsl:value-of select="docname"/>
									</a>
								</td>
								<td>
									<a id="{Номенклатура}" name="Довідник.Номенклатура" href="/">
										<xsl:value-of select="Номенклатура_Назва"/>
									</a>
								</td>
								<td>
									<a id="{ХарактеристикаНоменклатури}" name="Довідник.Характеристика" href="/">
										<xsl:value-of select="ХарактеристикаНоменклатури_Назва"/>
									</a>
								</td>
								<td>
									<a id="{Склад}" name="Довідник.Склад" href="/">
										<xsl:value-of select="Склад_Назва"/>
									</a>
								</td>
								<td style="text-align:center;width:30;">
									<xsl:choose>
										<xsl:when test="income='True'">+</xsl:when>
										<xsl:otherwise>-</xsl:otherwise>
									</xsl:choose>
								</td>
								<td align="right">
									<xsl:value-of select="ВРезервіЗіСкладу"/>
								</td>
								<td align="right">
									<xsl:value-of select="ВРезервіПідЗамовлення"/>
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
