﻿<?xml version="1.0" encoding="utf-8"?>
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

					<h4>Партії товарів: документи</h4>
					<p>
						Період з <xsl:value-of select="head/row/ПочатокПеріоду"/> по <xsl:value-of select="head/row/КінецьПеріоду"/>
					</p>

					<table class="table table-bordered table-sm table-hover">
						<tr class="table-success">
							<th width="20%" style="vertical-align:middle">Документ</th>
							<th width="10%" style="vertical-align:middle">Організація</th>
							<th width="20%" style="vertical-align:middle">Партія товарів</th>
							<th width="10%" style="vertical-align:middle">Номенклатура</th>
							<th width="10%" style="vertical-align:middle">Характеристика</th>
							<th width="10%" style="vertical-align:middle">Серія</th>
							<th width="9%" style="vertical-align:middle">Склад</th>
							<th width="1%" style="vertical-align:middle">...</th>
							<th width="5%" style="text-align:center;vertical-align:middle">Кількість</th>
							<th width="5%" style="text-align:center;vertical-align:middle">Собівартість</th>
						</tr>

						<xsl:for-each select="Документи/row">
							<tr>
								<td>
									<a id="{uid}" name="Документ.{doctype}" href="/">
										<xsl:value-of select="docname"/>
									</a>
								</td>
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
									<a id="{Серія}" name="Довідник.СеріїНоменклатури" href="/">
										<xsl:value-of select="Серія_Номер"/>
									</a>
								</td>
								<td>
									<a id="{Склад}" name="Довідник.Склади" href="/">
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
